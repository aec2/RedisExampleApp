using RedisExampleApp.API.Model;

namespace RedisExampleApp.API.Repositories
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int id);
    }
}