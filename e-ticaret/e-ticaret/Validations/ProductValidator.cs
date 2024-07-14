using e_ticaret.Models;
using FluentValidation;

namespace e_ticaret.Validations
{
    public class ProductValidator:AbstractValidator<ProductViewModel>
    {
        public ProductValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Bu alan boş olamaz!");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("En az 3 karekter içermelidir!");
            RuleFor(x => x.Price).NotEmpty().WithMessage("bu alan boş olamaz!");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Lütfen bir kategori seçiniz!");
           
            
        }
    }
}
