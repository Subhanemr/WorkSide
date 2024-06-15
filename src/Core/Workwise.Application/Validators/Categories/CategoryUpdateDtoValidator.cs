using FluentValidation;
using Workwise.Application.Dtos;

namespace Workwise.Application.Validators.Categories
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 50).WithMessage("Name max characters is 2-50")
                .Matches(@"^[a-zA-Z0-9\s]*$").WithMessage("Name can only contain letters, numbers, and spaces");
        }
    }
}
