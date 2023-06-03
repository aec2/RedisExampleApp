using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.API.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Kalem 1", Price = 10.00M },
                new Product() { Id = 2, Name = "Notebook 2", Price = 15.00M },
                new Product() { Id = 3, Name = "Pen 3", Price = 7.00M },
                new Product() { Id = 4, Name = "Pencil 4", Price = 3.00M },
                new Product() { Id = 5, Name = "Eraser 5", Price = 1.50M },
                new Product() { Id = 6, Name = "Ruler 6", Price = 2.00M },
                new Product() { Id = 7, Name = "Highlighter 7", Price = 8.00M },
                new Product() { Id = 8, Name = "Folder 8", Price = 6.50M },
                new Product() { Id = 9, Name = "Marker 9", Price = 4.00M },
                new Product() { Id = 10, Name = "Stapler 10", Price = 12.00M },
                new Product() { Id = 11, Name = "Paper Clips 11", Price = 1.00M }
                );
            base.OnModelCreating(modelBuilder);
        }

    }
}
