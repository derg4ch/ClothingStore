using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ProductDetail ProductDetail { get; set; }
        public ICollection<ProductSupplier> ProductSuppliers { get; set; }
    }
}

