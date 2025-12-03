using ClothingStoreOrders.BLL.DTO;
using ClothingStoreOrders.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStoreOrders.BLL.Services.Interfaces
{
    public interface ICustomerService
    {
        Task AddCustomerAsync(CustomerDto customerDto);
        Task<CustomerDto> GetCustomerByIdAsync(int customerId);
        Task<CustomerDto> GetCustomerByEmailAsync(string email);
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task UpdateCustomerAsync(CustomerDto customerDto);
        Task DeleteCustomerAsync(int customerId);
    }
}
