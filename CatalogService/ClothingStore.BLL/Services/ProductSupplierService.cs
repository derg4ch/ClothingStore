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
    public class ProductSupplierService : IProductSupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductSupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductSupplierDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.ProductSuppliers.GetAllAsync();
            return _mapper.Map<List<ProductSupplierDto>>(entities);
        }

        public async Task<ProductSupplierDto?> GetByIdsAsync(int productId, int supplierId)
        {
            var all = await _unitOfWork.ProductSuppliers.GetAllAsync();
            var entity = all.FirstOrDefault(ps => ps.ProductId == productId && ps.SupplierId == supplierId);

            return entity != null ? _mapper.Map<ProductSupplierDto>(entity) : null;
        }

        public async Task<ProductSupplierDto> CreateAsync(ProductSupplierDto dto)
        {
            var entity = _mapper.Map<ProductSupplier>(dto);
            entity.SupplyDate = DateTime.UtcNow;
            await _unitOfWork.ProductSuppliers.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductSupplierDto>(entity);
        }

        public async Task DeleteAsync(int productId, int supplierId)
        {
            var all = await _unitOfWork.ProductSuppliers.GetAllAsync();
            var entity = all.FirstOrDefault(ps => ps.ProductId == productId && ps.SupplierId == supplierId);

            if (entity == null)
                throw new Exception("ProductSupplier link not found");

            _unitOfWork.ProductSuppliers.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetProductsBySupplierIdAsync(int supplierId)
        {
            var productSuppliers = await _unitOfWork.ProductSuppliers.GetBySupplierIdAsync(supplierId);
            var products = productSuppliers.Select(ps => ps.Product).ToList();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<SupplierDto>> GetSuppliersByProductIdAsync(int productId)
        {
            var productSuppliers = await _unitOfWork.ProductSuppliers.GetByProductIdAsync(productId);
            var suppliers = productSuppliers.Select(ps => ps.Supplier).ToList();
            return _mapper.Map<List<SupplierDto>>(suppliers);
        }
    }
}

