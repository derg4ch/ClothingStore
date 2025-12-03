using ClothingStore.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Services.Interfaces
{
    public interface IProductDetailService
    {
        Task<List<ProductDetailDto>> GetAllAsync();
        Task<ProductDetailDto?> GetByIdAsync(int id);
        Task<ProductDetailDto?> GetByProductIdAsync(int productId);
        Task<ProductDetailDto> CreateAsync(ProductDetailCreateDto dto);
        Task UpdateAsync(int id, ProductDetailCreateDto dto);
        Task DeleteAsync(int id);
        Task<List<ProductDetailDto>> GetByBrandAsync(string brand);
        Task<List<ProductDetailDto>> GetLowStockProductsAsync(int threshold);
    }
}

