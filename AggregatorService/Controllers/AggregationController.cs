using AggregatorService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/aggregation")]
    public class AggregationController : ControllerBase
    {
        private readonly IAggregationService _service;

        public AggregationController(IAggregationService service)
        {
            _service = service;
        }

        [HttpGet("order/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFullOrder(int orderId)
        {
            try
            {
                var result = await _service.GetOrderWithReviewAsync(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("order/{orderId}/full")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFullOrderWithProductDetails(int orderId)
        {
            try
            {
                var result = await _service.GetOrderWithProductDetailsAsync(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
