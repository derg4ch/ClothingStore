using ClothingStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Repositories.Intarfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Product>> GetProductsByCategoryNameAsync(string categoryName);
    }
}
