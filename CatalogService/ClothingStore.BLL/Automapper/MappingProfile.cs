using AutoMapper;
using ClothingStore.BLL.DTO;
using ClothingStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            
            // Налаштування мапінгу для створення Category з CategoryDto
            // Встановлюємо порожній рядок для Description, якщо він не передано
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => 
                    string.IsNullOrWhiteSpace(src.Description) ? string.Empty : src.Description))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<ProductCreateDto, Product>();
            
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Supplier, SupplierCreateDto>().ReverseMap();
            
            CreateMap<ProductDetail, ProductDetailDto>().ReverseMap();
            CreateMap<ProductDetailCreateDto, ProductDetail>().ReverseMap();
            
            CreateMap<ProductSupplier, ProductSupplierDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
                .ReverseMap();
        }

    }
}
