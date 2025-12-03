using ClothingStore.BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.BLL.Validation
{
    public class SupplierCreateDtoValidator : AbstractValidator<SupplierCreateDto>
    {
        public SupplierCreateDtoValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Supplier name is required")
                .MaximumLength(150).WithMessage("Name cannot exceed 150 characters");

            RuleFor(s => s.Email)
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(s => s.Phone)
                .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters");

            RuleFor(s => s.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");
        }
    }
}
