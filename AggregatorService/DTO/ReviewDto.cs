using System.Text.Json.Serialization;

namespace AggregatorService.DTO
{
    public class ReviewDto
    {
        [JsonPropertyName("_id")] 
        public string Id { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? OrderId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsVerifiedPurchase { get; set; }
        public int HelpfulCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
