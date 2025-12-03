using AutoMapper;
using ClothingStore.BLL.DTO;
using ClothingStore.BLL.Services.Interfaces;
using ClothingStore.DAL.Entities;
using ClothingStore.DAL.QueryParametrs;
using ClothingStore.DAL.Specefication;
using ClothingStore.DAL.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDto>> GetProductsAsync(ProductQueryParameters parameters)
        {
            var result = await _unitOfWork.Products.GetProductsAsync(parameters);
            var productDtos = _mapper.Map<List<ProductDto>>(result.Items);
            return new PagedResult<ProductDto>(productDtos, result.TotalCount, result.PageSize);
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Одяг не знайдено");

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Назва одягу не може бути порожньою!");

            if (dto.Price <= 0)
                throw new ArgumentException("Ціна повинна бути більшою за 0!");

            if (string.IsNullOrWhiteSpace(dto.SKU))
                throw new ArgumentException("SKU не може бути порожнім!");

            var product = _mapper.Map<Product>(dto);
            product.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateAsync(int id, ProductCreateDto dto)
        {
            var existing = await _unitOfWork.Products.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Одяг не знайдено");

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.SKU = dto.SKU;
            existing.CategoryId = dto.CategoryId;
            existing.IsActive = dto.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Products.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Products.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Одяг не знайдено");

            _unitOfWork.Products.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> SearchByNameAsync(string keyword)
        {
            var result = await _unitOfWork.Products.SearchByNameAsync(keyword);
            return _mapper.Map<List<ProductDto>>(result);
        }

        public async Task<List<ProductDto>> GetProductsAbovePriceAsync(decimal price)
        {
            var products = await _unitOfWork.Products.GetProductsAbovePriceAsync(price);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ProductDto>> GetProductsBelowPriceAsync(decimal price)
        {
            var products = await _unitOfWork.Products.GetProductsBelowPriceAsync(price);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ProductDto>> GetActiveProductsAsync()
        {
            var products = await _unitOfWork.Products.GetActiveProductsAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}

