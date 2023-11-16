using RedisApp.API.Model;
using RedisApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisApp.API.Repositories
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private readonly IProductRepository _productepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;
        private const string productKey = "productCaches";
        public ProductRepositoryWithCache(IProductRepository productepository, RedisService redisService)
        {
            _productepository = productepository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(2);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productepository.CreateAsync(product);

            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));
            }

            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();
            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }
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

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productepository.GetAsync();

            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });

            return products;
        }


    }
}
