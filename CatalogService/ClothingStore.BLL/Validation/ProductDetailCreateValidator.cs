using ClothingStore.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Validation
{
    public class ProductDetailCreateDtoValidator : AbstractValidator<ProductDetailCreateDto>
    {
        public ProductDetailCreateDtoValidator()
        {
            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Бренд не може бути порожнім")
                .MaximumLength(100).WithMessage("Бренд не може перевищувати 100 символів");

            RuleFor(x => x.Material)
                .NotEmpty().WithMessage("Матеріал не може бути порожнім")
                .MaximumLength(100).WithMessage("Матеріал не може перевищувати 100 символів");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Колір не може бути порожнім")
                .MaximumLength(50).WithMessage("Колір не може перевищувати 50 символів");

            RuleFor(x => x.Size)
                .NotEmpty().WithMessage("Розмір не може бути порожнім")
                .MaximumLength(20).WithMessage("Розмір не може перевищувати 20 символів");

            RuleFor(x => x.CareInstructions)
                .MaximumLength(500).WithMessage("Інструкції з догляду не можуть перевищувати 500 символів");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Кількість на складі не може бути від'ємною");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId повинен бути більшим за 0");
        }
    }
}

