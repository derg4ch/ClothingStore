using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Entities
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProductSupplier> ProductSuppliers { get; set; }
    }
}
