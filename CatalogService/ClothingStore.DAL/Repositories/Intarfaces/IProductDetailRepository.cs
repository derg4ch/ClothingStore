using ClothingStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Repositories.Intarfaces
{
    public interface IProductDetailRepository : IGenericRepository<ProductDetail>
    {
        Task<ProductDetail> GetByProductIdAsync(int productId);
        Task<List<ProductDetail>> GetLowStockProductsAsync(int threshold);
    }
}

