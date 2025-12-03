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
    public class ProductDetailRepository : GenericRepository<ProductDetail>, IProductDetailRepository
    {
        public ProductDetailRepository(ClothingStoreContext context) : base(context) { }

        public async Task<ProductDetail> GetByProductIdAsync(int productId)
        {
            return await _context.ProductDetails
                .FirstOrDefaultAsync(pd => pd.ProductId == productId);
        }

        public async Task<List<ProductDetail>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.ProductDetails
                .Where(pd => pd.StockQuantity <= threshold)
                .Include(pd => pd.Product)
                .ToListAsync();
        }
    }
}

