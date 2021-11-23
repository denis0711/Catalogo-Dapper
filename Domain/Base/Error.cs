using Domain.Enums;

namespace Domain.Base
{
    public class Error
    {
        public string Campo { get; private set; }
        public string Mensagem { get; private set; }
        public object Valor { get; private set; }
        public TipoErro TipoErro { get; set; }

        public static Error Criar(string campo, string mensagem, TipoErro tipoErro, object valor)
        {
            var erroValidacao = new Error()
            {
                Campo = campo,
                Mensagem = mensagem,
                TipoErro = tipoErro,
                Valor = valor
            };

            return erroValidacao;
        }
    }
}
