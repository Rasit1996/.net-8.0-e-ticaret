using e_ticaret.Models;
using FluentValidation;

namespace e_ticaret.Validations
{
	public class LoginValidator:AbstractValidator<LoginViewModel>
	{
        public LoginValidator()
        {
            RuleFor(x => x.email).NotEmpty().WithMessage("Boş geçilemez!");
            RuleFor(x => x.password).NotEmpty().WithMessage("Boş geçilemez");
        }
    }
}
