using ClothingStore.DAL.Db;
using ClothingStore.DAL.Repositories.Intarfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClothingStoreContext _context;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public ISupplierRepository Suppliers { get; }
        public IProductDetailRepository ProductDetails { get; }
        public IProductSupplierRepository ProductSuppliers { get; }

        public UnitOfWork(
            ClothingStoreContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository,
            IProductSupplierRepository productSupplierRepository,
            IProductDetailRepository productDetailRepository)
        {
            _context = context;
            Products = productRepository;
            Categories = categoryRepository;
            Suppliers = supplierRepository;
            ProductDetails = productDetailRepository;
            ProductSuppliers = productSupplierRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
