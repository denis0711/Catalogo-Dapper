using Domain.Base;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Categoria : BaseEntity
    {
        //Campos de retorno sem exposição na API
        public int Id_Categoria { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; }
        public IEnumerable<Produto>  Produtos { get; set; }


    }

}
