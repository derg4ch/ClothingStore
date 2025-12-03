using ClothingStore.BLL.DTO;
using ClothingStore.BLL.Services.Interfaces;
using ClothingStore.DAL.QueryParametrs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.API.Controllers
{
    [Route("api/Catalog/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParameters parameters)
        {
            var products = await _productService.GetProductsAsync(parameters);
            return Ok(products);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveProducts()
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            try
            {
                var created = await _productService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCreateDto dto)
        {
            try
            {
                await _productService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var products = await _productService.SearchByNameAsync(keyword);
            return Ok(products);
        }

        [HttpGet("price/above/{price}")]
        public async Task<IActionResult> GetProductsAbovePrice(decimal price)
        {
            var products = await _productService.GetProductsAbovePriceAsync(price);
            if (products == null || !products.Any())
                return NotFound("No products found above this price");

            return Ok(products);
        }

        [HttpGet("price/below/{price}")]
        public async Task<IActionResult> GetProductsBelowPrice(decimal price)
        {
            var products = await _productService.GetProductsBelowPriceAsync(price);
            if (products == null || !products.Any())
                return NotFound("No products found below this price");

            return Ok(products);
        }
    }
}

