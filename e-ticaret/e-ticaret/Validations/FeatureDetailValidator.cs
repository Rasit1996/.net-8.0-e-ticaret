using e_ticaret.Models;
using FluentValidation;

namespace e_ticaret.Validations
{
    public class FeatureDetailValidator:AbstractValidator<FeatureDetailViewModel>
    {
        public FeatureDetailValidator()
        {
            RuleFor(x => x.property).NotEmpty().WithMessage("Bu alan boş olamaz!");
           
        }
    }
}
