using AggregatorService.DTO;

namespace AggregatorService.Services
{
    public interface IAggregationService
    {
        Task<OrderWithReviewDto> GetOrderWithReviewAsync(int orderId);
        Task<OrderWithReviewDto> GetOrderWithProductDetailsAsync(int orderId);
    }

}
