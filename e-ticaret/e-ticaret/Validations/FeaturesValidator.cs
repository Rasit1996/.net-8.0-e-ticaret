using e_ticaret.Models;
using FluentValidation;

namespace e_ticaret.Validations
{
    public class FeaturesValidator:AbstractValidator<FeaturesViewModel>
    {
        public FeaturesValidator()
        {
            RuleFor(x=>x.name).NotEmpty().WithMessage("Bu alan boş olamaz.");
            RuleFor(x => x.title).NotEmpty().WithMessage("Bu alan boş olamaz.");
        }
    }
}
