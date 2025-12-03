using ClothingStore.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Validation
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва одягу не може бути порожньою")
                .MaximumLength(200).WithMessage("Назва одягу не може перевищувати 200 символів");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Опис не може перевищувати 1000 символів");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ціна повинна бути більшою за 0")
                .LessThanOrEqualTo(10000).WithMessage("Ціна не може перевищувати 10000");

            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU не може бути порожнім")
                .MaximumLength(50).WithMessage("SKU не може перевищувати 50 символів")
                .Matches(@"^[A-Z0-9-]+$").WithMessage("SKU може містити тільки великі літери, цифри та дефіси");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId повинен бути більшим за 0");
        }
    }
}

