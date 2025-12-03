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
    public class ProductDetailService : IProductDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductDetailDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.ProductDetails.GetAllAsync();
            return _mapper.Map<List<ProductDetailDto>>(entities);
        }

        public async Task<ProductDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.ProductDetails.GetByIdAsync(id);
            return _mapper.Map<ProductDetailDto>(entity);
        }

        public async Task<ProductDetailDto?> GetByProductIdAsync(int productId)
        {
            var entity = await _unitOfWork.ProductDetails.GetByProductIdAsync(productId);
            return _mapper.Map<ProductDetailDto>(entity);
        }

        public async Task<ProductDetailDto> CreateAsync(ProductDetailCreateDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            var entity = _mapper.Map<ProductDetail>(dto);
            entity.ProductId = dto.ProductId;

            await _unitOfWork.ProductDetails.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDetailDto>(entity);
        }

        public async Task UpdateAsync(int id, ProductDetailCreateDto dto)
        {
            var existing = await _unitOfWork.ProductDetails.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("ProductDetail not found");

            existing.Brand = dto.Brand;
            existing.Material = dto.Material;
            existing.Color = dto.Color;
            existing.Size = dto.Size;
            existing.CareInstructions = dto.CareInstructions;
            existing.StockQuantity = dto.StockQuantity;

            _unitOfWork.ProductDetails.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.ProductDetails.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("ProductDetail not found");

            _unitOfWork.ProductDetails.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ProductDetailDto>> GetByBrandAsync(string brand)
        {
            var details = await _unitOfWork.ProductDetails.GetAllAsync();
            var filtered = details.Where(d => d.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase)).ToList();
            return _mapper.Map<List<ProductDetailDto>>(filtered);
        }

        public async Task<List<ProductDetailDto>> GetLowStockProductsAsync(int threshold)
        {
            var details = await _unitOfWork.ProductDetails.GetLowStockProductsAsync(threshold);
            return _mapper.Map<List<ProductDetailDto>>(details);
        }
    }
}

