using Domain.Entities;
using FluentValidation;

namespace Services.Validators
{
    public class CatalogoValidator : AbstractValidator<Catalogo>
    {
        public CatalogoValidator()
        {
            ////EXEMPLO
            //RuleFor(c => c.Login)
            //      .NotEmpty()
            //      .WithMessage("Login n�o pode ser nulo.");
        }
    }
}
