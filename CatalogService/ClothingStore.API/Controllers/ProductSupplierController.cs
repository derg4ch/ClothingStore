using ClothingStore.BLL.DTO;
using ClothingStore.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.API.Controllers
{
    [Route("api/Catalog/[controller]")]
    [ApiController]
    public class ProductSupplierController : ControllerBase
    {
        private readonly IProductSupplierService _service;

        public ProductSupplierController(IProductSupplierService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var links = await _service.GetAllAsync();
            return Ok(links);
        }

        [HttpGet("{productId}/{supplierId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIds(int productId, int supplierId)
        {
            var link = await _service.GetByIdsAsync(productId, supplierId);
            if (link == null)
                return NotFound();

            return Ok(link);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductSupplierDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByIds), new { productId = created.ProductId, supplierId = created.SupplierId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{productId}/{supplierId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int productId, int supplierId)
        {
            try
            {
                await _service.DeleteAsync(productId, supplierId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("product/{productId}/suppliers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSuppliersByProduct(int productId)
        {
            var suppliers = await _service.GetSuppliersByProductIdAsync(productId);
            return Ok(suppliers);
        }

        [HttpGet("supplier/{supplierId}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsBySupplier(int supplierId)
        {
            var products = await _service.GetProductsBySupplierIdAsync(supplierId);
            return Ok(products);
        }
    }
}

