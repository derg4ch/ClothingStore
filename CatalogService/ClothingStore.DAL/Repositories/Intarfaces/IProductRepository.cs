using ClothingStore.DAL.Entities;
using ClothingStore.DAL.QueryParametrs;
using ClothingStore.DAL.Specefication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Repositories.Intarfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsAbovePriceAsync(decimal price);
        Task<List<Product>> GetProductsBelowPriceAsync(decimal price);
        Task<List<Product>> SearchByNameAsync(string keyword);
        Task<List<Product>> GetActiveProductsAsync();

        Task<PagedResult<Product>> GetProductsAsync(ProductQueryParameters parameters);
    }
}

