using ClothingStore.BLL.DTO;
using ClothingStore.DAL.QueryParametrs;
using ClothingStore.DAL.Specefication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductCreateDto dto);
        Task UpdateAsync(int id, ProductCreateDto dto);
        Task DeleteAsync(int id);
        Task<List<ProductDto>> SearchByNameAsync(string keyword);
        Task<List<ProductDto>> GetProductsAbovePriceAsync(decimal price);
        Task<List<ProductDto>> GetProductsBelowPriceAsync(decimal price);
        Task<PagedResult<ProductDto>> GetProductsAsync(ProductQueryParameters parameters);
        Task<List<ProductDto>> GetActiveProductsAsync();
    }
}

