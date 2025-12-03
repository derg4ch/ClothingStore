using ClothingStoreOrders.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStoreOrders.DAL.Repositories.Interfaces
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>
    {
        Task<OrderDetails?> GetByOrderIdAsync(int orderId);
    }
}
