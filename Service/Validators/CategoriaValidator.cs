using Domain.Entities;
using FluentValidation;

namespace Services.Validators
{
    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        public CategoriaValidator()
        {

            RuleFor(c => c.Nome).NotEmpty().WithMessage("O campo precisa ser Fornecido");
            RuleFor(p => p.ImagemUrl).NotEmpty().WithMessage("O campo precisa ser preenchido");
            ////EXEMPLO
            //RuleFor(c => c.Login)
            //      .NotEmpty()
            //      .WithMessage("Login não pode ser nulo.");
        }
    }
}
