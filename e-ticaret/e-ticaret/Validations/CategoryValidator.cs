using e_ticaret.Models;
using FluentValidation;

namespace e_ticaret.Validations
{
    public class CategoryValidator:AbstractValidator<CategoryViewModel>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.name).MinimumLength(3).WithMessage("Kategori ismi en az 3 karekter içermelidir!");
            RuleFor(x => x.name).NotEmpty().WithMessage("Kategori adı boş olamaz!");
        }
    }
}
