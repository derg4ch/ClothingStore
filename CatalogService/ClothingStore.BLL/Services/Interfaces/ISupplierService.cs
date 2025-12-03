using ClothingStore.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync();
        Task<SupplierDto> GetByIdAsync(int id);
        Task<SupplierDto> CreateAsync(SupplierCreateDto dto);
        Task UpdateAsync(int id, SupplierCreateDto dto);
        Task DeleteAsync(int id);
        Task<SupplierDto> GetSupplierWithProductsAsync(int id);
    }
}
