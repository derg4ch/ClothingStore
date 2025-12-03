using ClothingStore.DAL.Db;
using ClothingStore.DAL.Entities;
using ClothingStore.DAL.Repositories.Intarfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Repositories
{
    public class ProductSupplierRepository : GenericRepository<ProductSupplier>, IProductSupplierRepository
    {
        public ProductSupplierRepository(ClothingStoreContext context) : base(context) { }

        public async Task<List<ProductSupplier>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductSuppliers
                .Where(ps => ps.ProductId == productId)
                .Include(ps => ps.Supplier)
                .ToListAsync();
        }

        public async Task<List<ProductSupplier>> GetBySupplierIdAsync(int supplierId)
        {
            return await _context.ProductSuppliers
                .Where(ps => ps.SupplierId == supplierId)
                .Include(ps => ps.Product)
                .ToListAsync();
        }
    }
}

