using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public class Resultado<T>
    {
        public T Conteudo { get; }
        public List<Error> Erros { get; set; }
        public bool BadRequest { get; set; }

        private Resultado(T conteudo, List<Error> erros)
        {
            Conteudo = conteudo;
            if (erros?.Count > 0)
            {
                BadRequest = true;
                Erros = erros;
            }
        }

        public static Resultado<T> ComSucesso(T conteudo) => new Resultado<T>(conteudo, null);

        public static Resultado<T> ComErros(T conteudo, List<Error> erros) => new Resultado<T>(conteudo, erros);

        public static List<Error> AdicionarErro(Error erro)
        {
            return new List<Error>() { erro };
        }
    }
}
