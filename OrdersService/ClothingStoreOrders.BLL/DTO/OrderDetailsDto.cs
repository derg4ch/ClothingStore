using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStoreOrders.BLL.DTO
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public string ShippingMethod { get; set; }
        public string TrackingNumber { get; set; }
        public string Notes { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
    }
}
