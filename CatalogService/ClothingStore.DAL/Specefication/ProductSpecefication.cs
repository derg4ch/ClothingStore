using ClothingStore.DAL.Entities;
using ClothingStore.DAL.QueryParametrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Specefication
{
    public class ProductSpecification : Specification<Product>
    {
        public ProductSpecification(ProductQueryParameters parameters)
        {
            Criteria = p =>
                (string.IsNullOrEmpty(parameters.SearchTerm) ||
                    p.Name.Contains(parameters.SearchTerm) ||
                    p.Description.Contains(parameters.SearchTerm)) &&
                (!parameters.CategoryId.HasValue ||
                    p.CategoryId == parameters.CategoryId.Value) &&
                (!parameters.MinPrice.HasValue ||
                    p.Price >= parameters.MinPrice.Value) &&
                (!parameters.MaxPrice.HasValue ||
                    p.Price <= parameters.MaxPrice.Value) &&
                (!parameters.IsActive.HasValue ||
                    p.IsActive == parameters.IsActive.Value);

            // Include залежностей
            Includes.Add(p => p.Category);
            Includes.Add(p => p.ProductDetail);
            Includes.Add(p => p.ProductSuppliers);

            // Сортування
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                var isDesc = parameters.SortDirection?.ToLower() == "desc";
                
                if (isDesc)
                {
                    OrderByDescending = parameters.SortBy.ToLower() switch
                    {
                        "name" => q => q.OrderByDescending(p => p.Name),
                        "price" => q => q.OrderByDescending(p => p.Price),
                        "createdat" => q => q.OrderByDescending(p => p.CreatedAt),
                        _ => OrderByDescending
                    };
                }
                else
                {
                    OrderBy = parameters.SortBy.ToLower() switch
                    {
                        "name" => q => q.OrderBy(p => p.Name),
                        "price" => q => q.OrderBy(p => p.Price),
                        "createdat" => q => q.OrderBy(p => p.CreatedAt),
                        _ => OrderBy
                    };
                }
            }
            else
            {
                OrderBy = q => q.OrderBy(p => p.Name);
            }

            Skip = (parameters.Page - 1) * parameters.PageSize;
            Take = parameters.PageSize;
        }
    }
}

