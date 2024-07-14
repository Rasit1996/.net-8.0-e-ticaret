using e_ticaret.Models;
using FluentValidation;

namespace e_ticaret.Validations
{
    public class RegisterValidater:AbstractValidator<RegisterViewModel>
    {
        public RegisterValidater()
        {
            RuleFor(x => x.name).MinimumLength(3).WithMessage("Ad en az 3 karekter oluşmalıdır!");
            RuleFor(x => x.surname).MinimumLength(3).WithMessage("Soyad en az 3 karakterden oluşmalıdır!");
            RuleFor(x => x.email).NotEmpty().WithMessage("Bu alan boş geçilemez!");
            RuleFor(x => x.password).NotEmpty().WithMessage("Şifre gerekli!");
            RuleFor(x => x.confirmPassword).Equal(x => x.password).WithMessage("Şifreler uyuşmuyor");
        }
    }
}
