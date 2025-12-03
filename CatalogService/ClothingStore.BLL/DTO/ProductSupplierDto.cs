using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.DTO
{
    public class ProductSupplierDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal SupplierPrice { get; set; }
        public DateTime SupplyDate { get; set; }
    }
}

