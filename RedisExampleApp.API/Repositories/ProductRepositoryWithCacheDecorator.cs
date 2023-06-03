using System.Text.Json;
using RedisExampleApp.API.Model;
using RedisExampleApp.Cache;
using StackExchange.Redis;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;

        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(2);
        }
        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productRepository.CreateAsync(product);

            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, newProduct.Id,
                    JsonSerializer.Serialize<Product>(newProduct));
                return newProduct;
            }

            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            var cacheProducts= await _cacheRepository.HashGetAllAsync(productKey);
            var products = cacheProducts.ToList().Select(cacheProduct => JsonSerializer.Deserialize<Product>(cacheProduct.Value)).ToList();

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (_cacheRepository.KeyExists(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey,id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAsync();

            products.ForEach(product =>
            {
                _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product));
            });

            return products;
        }

    }
}
