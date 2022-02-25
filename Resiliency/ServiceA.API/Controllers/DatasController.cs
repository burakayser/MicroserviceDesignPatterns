using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceA.API.Services;
using System.Threading.Tasks;

namespace ServiceA.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DatasController : ControllerBase
    {

        private readonly ProductService _productService;

        public DatasController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _productService.GetProduct(id));
        }
    }
}
