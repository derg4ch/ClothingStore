namespace AggregatorService.DTO
{
    public class OrderWithReviewDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }

        public IEnumerable<OrderItemDto> Items { get; set; }

        public IEnumerable<ReviewDto> Reviews { get; set; }
    }
}
