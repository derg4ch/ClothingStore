using AutoMapper;
using ClothingStoreOrders.BLL.DTO;
using ClothingStoreOrders.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStoreOrders.BLL.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItemProductInfo, OrderItemWithProductDto>();
        }

    }
}

