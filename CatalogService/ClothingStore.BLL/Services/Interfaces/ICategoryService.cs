using ClothingStore.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> AddCategoryAsync(CategoryDto categoryDto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, CategoryDto dto);
        Task<List<ProductDto>> GetProductsByCategoryNameAsync(string categoryName);

    }
}
