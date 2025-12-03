using ClothingStore.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Validation
{
    public class ProductSupplierCreateValidator : AbstractValidator<ProductSupplierDto>
    {
        public ProductSupplierCreateValidator()
        {
            RuleFor(ps => ps.ProductId)
                .GreaterThan(0).WithMessage("ProductId повинен бути більшим за 0");

            RuleFor(ps => ps.SupplierId)
                .GreaterThan(0).WithMessage("SupplierId повинен бути більшим за 0");

            RuleFor(ps => ps.SupplierPrice)
                .GreaterThan(0).WithMessage("Ціна постачальника повинна бути більшою за 0");
        }
    }
}

