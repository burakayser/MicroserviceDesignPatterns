using Microsoft.Extensions.Logging;
using ServiceA.API.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ServiceA.API.Services
{
    public class ProductService
    {

        private readonly HttpClient _client;
        private readonly ILogger<ProductService> _logger;

        public ProductService(HttpClient client, ILogger<ProductService> logger)
        {
            _client = client;
            _logger = logger;
        }


        public async Task<Product> GetProduct(int id)
        {
            var product = await _client.GetFromJsonAsync<Product>($"{id}");

            _logger.LogInformation($"Proudcts: {product.Id}-{product.Name}");

            return product;

        }

    }
}
