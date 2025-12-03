using ClothingStoreOrders.DAL.Models;
using ClothingStoreOrders.DAL.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStoreOrders.DAL.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(IDbConnection connection, IDbTransaction transaction = null)
            : base(connection, "OrderItems", "OrderItemId", transaction)
        {
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
        {
            string sql = "SELECT * FROM OrderItems WHERE OrderId = @OrderId";
            return await _connection.QueryAsync<OrderItem>(sql, new { OrderId = orderId }, transaction: _transaction);
        }

        public async Task<IEnumerable<OrderItemProductInfo>> GetOrderItemsWithProductAsync()
        {
            string sql = @"
            SELECT 
            oi.OrderItemId,
            oi.OrderId,
            oi.ProductId,
            oi.Quantity,
            p.Name,
            p.Price
            FROM OrderItems oi
            INNER JOIN Products p ON oi.ProductId = p.ProductId";

            return await _connection.QueryAsync<OrderItemProductInfo>(sql, transaction: _transaction);
        }
    }
}
