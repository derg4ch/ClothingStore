using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Entities
{
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }
        public string Brand { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string CareInstructions { get; set; }
        public int StockQuantity { get; set; } = 0;

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

