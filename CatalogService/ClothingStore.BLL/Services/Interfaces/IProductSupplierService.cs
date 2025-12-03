using ClothingStore.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Services.Interfaces
{
    public interface IProductSupplierService
    {
        Task<List<ProductSupplierDto>> GetAllAsync();
        Task<ProductSupplierDto?> GetByIdsAsync(int productId, int supplierId);
        Task<ProductSupplierDto> CreateAsync(ProductSupplierDto dto);
        Task DeleteAsync(int productId, int supplierId);
        Task<List<SupplierDto>> GetSuppliersByProductIdAsync(int productId);
        Task<List<ProductDto>> GetProductsBySupplierIdAsync(int supplierId);
    }
}

