using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.DTO
{
    public class ProductDetailCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Brand { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Material { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Color { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Size { get; set; } = null!;

        [StringLength(500)]
        public string CareInstructions { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; } = 0;

        [Required]
        public int ProductId { get; set; }
    }
}

