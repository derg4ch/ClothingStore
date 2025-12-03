using AggregatorService.DTO;

namespace AggregatorService.Services
{
    public class AggregationService : IAggregationService
    {
        private readonly HttpClient _ordersClient;
        private readonly HttpClient _reviewsClient;
        private readonly HttpClient _catalogClient;

        public AggregationService(IHttpClientFactory factory)
        {
            _ordersClient = factory.CreateClient("orders");
            _reviewsClient = factory.CreateClient("reviews");
            _catalogClient = factory.CreateClient("catalog");
        }

        public async Task<OrderWithReviewDto> GetOrderWithReviewAsync(int orderId)
        {
            var order = await _ordersClient.GetFromJsonAsync<OrderWithReviewDto>($"api/Orders/Order/{orderId}");
            if (order == null)
                throw new Exception("Order not found");

            // Get reviews for all products in the order
            var productIds = order.Items?.Select(i => i.ProductId).ToList() ?? new List<int>();
            var allReviews = new List<ReviewDto>();

            foreach (var productId in productIds)
            {
                var reviews = await _reviewsClient.GetFromJsonAsync<List<ReviewDto>>($"api/Reviews/product/{productId}");
                if (reviews != null)
                {
                    allReviews.AddRange(reviews);
                }
            }

            order.Reviews = allReviews;

            return order;
        }

        public async Task<OrderWithReviewDto> GetOrderWithProductDetailsAsync(int orderId)
        {
            var order = await _ordersClient.GetFromJsonAsync<OrderWithReviewDto>($"api/Orders/Order/{orderId}");
            if (order == null)
                throw new Exception("Order not found");

            // Enrich order items with product details from catalog
            if (order.Items != null)
            {
                var enrichedItems = new List<OrderItemDto>();
                foreach (var item in order.Items)
                {
                    var response = await _catalogClient.GetAsync($"api/Catalog/Product/{item.ProductId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var productJson = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                        if (productJson.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            if (productJson.TryGetProperty("name", out var nameElement))
                                item.ProductName = nameElement.GetString() ?? item.ProductName;
                            if (productJson.TryGetProperty("sku", out var skuElement))
                                item.SKU = skuElement.GetString() ?? item.SKU;
                        }
                    }
                    enrichedItems.Add(item);
                }
                order.Items = enrichedItems;
            }

            // Get reviews
            var productIds = order.Items?.Select(i => i.ProductId).ToList() ?? new List<int>();
            var allReviews = new List<ReviewDto>();

            foreach (var productId in productIds)
            {
                var reviews = await _reviewsClient.GetFromJsonAsync<List<ReviewDto>>($"api/Reviews/product/{productId}");
                if (reviews != null)
                {
                    allReviews.AddRange(reviews);
                }
            }

            order.Reviews = allReviews;

            return order;
        }
    }
}
