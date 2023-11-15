using Microsoft.AspNetCore.Mvc;
using RedisApp.API.Model;
using RedisApp.API.Repositories;
using RedisApp.Cache;
using StackExchange.Redis;

namespace RedisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IDatabase _database;

        public ProductsController(IProductRepository productRepository, IDatabase database)
        {
            _productRepository = productRepository;
            _database = database;
            _database.StringSet("soyad", "Karagoz");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Created(string.Empty, await _productRepository.CreateAsync(product));
        }

    }
}
