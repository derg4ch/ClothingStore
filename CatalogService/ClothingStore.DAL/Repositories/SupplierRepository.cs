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
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ClothingStoreContext context) : base(context) { }
        public async Task<Supplier?> GetSupplierWithProductsAsync(int id)
        {
            return await _context.Suppliers
                .Include(s => s.ProductSuppliers)
                .ThenInclude(ps => ps.Product)
                .FirstOrDefaultAsync(s => s.SupplierId == id);
        }
        public async Task<List<Supplier>> SearchByNameAsync(string keyword)
        {
            return await _context.Suppliers
                .Where(s => s.Name.Contains(keyword))
                .ToListAsync();
        }
    }
}
