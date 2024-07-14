using e_ticaret.Models;
using FluentValidation;

namespace e_ticaret.Validations
{
    public class UserUpdateValidator:AbstractValidator<UserUpdateViewModel>
    {
        public UserUpdateValidator()
        {
            RuleFor(x => x.email).NotEmpty().WithMessage("Bu alan boş geçilemez!");
            RuleFor(x => x.name).MinimumLength(3).WithMessage("En az 3 karekter olmalıdır!");
            RuleFor(x=>x.surname).MinimumLength(3).WithMessage("En az 3 karekter olmalıdır!");
            RuleFor(x => x.name).NotEmpty().WithMessage("Bu alan boş geçilemez!");
            RuleFor(x => x.surname).NotEmpty().WithMessage("Bu alan boş geçilemez!");
        }
    }
}
