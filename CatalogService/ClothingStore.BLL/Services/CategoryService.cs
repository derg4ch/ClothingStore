using AutoMapper;
using ClothingStore.BLL.DTO;
using ClothingStore.BLL.Services.Interfaces;
using ClothingStore.DAL.Entities;
using ClothingStore.DAL.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<List<ProductDto>> GetProductsByCategoryNameAsync(string categoryName)
        {
            var products = await _unitOfWork.Categories.GetProductsByCategoryNameAsync(categoryName);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<CategoryDto> AddCategoryAsync(CategoryDto categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                throw new ArgumentException("Назва категорії не може бути порожньою!");

            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Categories.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.Categories.DeleteAsync(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(int id, CategoryDto dto)
        {
            var existing = await _unitOfWork.Categories.GetByIdAsync(id);

            if (existing == null)
            {
                throw new Exception("Категорію не знайдено");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Назва категорії не може бути порожньою!");
            }

            existing.Name = dto.Name;
            existing.Description = string.IsNullOrWhiteSpace(dto.Description) ? string.Empty : dto.Description;

            _unitOfWork.Categories.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
