using ClothingStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Repositories.Intarfaces
{
    public interface IProductSupplierRepository : IGenericRepository<ProductSupplier>
    {
        Task<List<ProductSupplier>> GetByProductIdAsync(int productId);
        Task<List<ProductSupplier>> GetBySupplierIdAsync(int supplierId);
    }
}

