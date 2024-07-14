using e_ticaret.Models;
using FluentValidation;

namespace e_ticaret.Validations
{
	public class MessageValidator:AbstractValidator<MessageViewModel>
	{
        public MessageValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen bir email adresi giriniz");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Bu alan boş geçilemez");
        }
    }
}
