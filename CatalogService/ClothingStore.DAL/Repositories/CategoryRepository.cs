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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ClothingStoreContext context) : base(context) { }

        public async Task<List<Product>> GetProductsByCategoryNameAsync(string categoryName)
        {
            return await _context.Categories
                .Where(c => c.Name == categoryName)
                .SelectMany(c => c.Products) 
                .ToListAsync();
        }
    }
}
