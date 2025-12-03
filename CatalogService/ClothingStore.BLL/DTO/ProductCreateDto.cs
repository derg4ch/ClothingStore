using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.DTO
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string SKU { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

