using ClothingStoreOrders.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStoreOrders.DAL.Repositories.Interfaces
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderItemProductInfo>> GetOrderItemsWithProductAsync();
    }
}
