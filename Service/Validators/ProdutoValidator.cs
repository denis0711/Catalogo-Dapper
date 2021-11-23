using Domain.Entities;
using FluentValidation;

namespace Services.Validators
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(c => c.Nome).NotEmpty().WithMessage("O campo precisa ser Fornecido");
            RuleFor(p => p.ImagemUrl).NotEmpty().WithMessage("O campo precisa ser preenchido");
            RuleFor(p => p.Preco).NotEmpty().WithMessage("O campo precisa ser preenchido");
            RuleFor(p => p.Id_Categoria).NotNull().NotEmpty();
            ////EXEMPLO
            //RuleFor(c => c.Login)
            //      .NotEmpty()
            //      .WithMessage("Login não pode ser nulo.");
        }
    }
}
