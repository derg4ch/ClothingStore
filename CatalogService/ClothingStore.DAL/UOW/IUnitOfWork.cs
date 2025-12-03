using ClothingStore.DAL.Repositories.Intarfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ISupplierRepository Suppliers { get; }
        IProductDetailRepository ProductDetails { get; }
        IProductSupplierRepository ProductSuppliers { get; }

        Task<int> SaveChangesAsync();
    }
}
