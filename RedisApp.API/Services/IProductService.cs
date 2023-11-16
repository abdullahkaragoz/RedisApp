using RedisApp.API.Model;

namespace RedisApp.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAsync();

        Task<Product> GetByIdAsync(int id);

        Task<Product> CreateAsync(Product product);
    }
}
