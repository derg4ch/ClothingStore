using ClothingStore.DAL.Db;
using ClothingStore.DAL.Entities;
using ClothingStore.DAL.QueryParametrs;
using ClothingStore.DAL.Repositories.Intarfaces;
using ClothingStore.DAL.Specefication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ClothingStoreContext context) : base(context) { }

        public async Task<PagedResult<Product>> GetProductsAsync(ProductQueryParameters parameters)
        {
            var spec = new ProductSpecification(parameters);

            var query = SpecificationEvaluator.GetQuery(_context.Products.AsQueryable(), spec);

            var totalCount = await _context.Products
                .Where(spec.Criteria)
                .CountAsync();

            var items = await query.ToListAsync();

            return new PagedResult<Product>(items, totalCount, parameters.PageSize);
        }

        public async Task<List<Product>> GetProductsAbovePriceAsync(decimal price)
        {
            return await _context.Products
                .Where(p => p.Price > price && p.IsActive)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsBelowPriceAsync(decimal price)
        {
            return await _context.Products
                .Where(p => p.Price < price && p.IsActive)
                .ToListAsync();
        }

        public async Task<List<Product>> SearchByNameAsync(string keyword)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(keyword) && p.IsActive)
                .ToListAsync();
        }

        public async Task<List<Product>> GetActiveProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.ProductDetail)
                .ToListAsync();
        }
    }
}

