using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.DTO
{
    public class ProductDetailDto
    {
        public int ProductDetailId { get; set; }
        public string Brand { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string CareInstructions { get; set; }
        public int StockQuantity { get; set; }
        public int ProductId { get; set; }
    }
}

