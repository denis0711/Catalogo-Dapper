using Domain.Base;
using System.Collections.Generic;

namespace Domain.DTO
{
    public class Categoria 
    {
        public int Id_Categoria { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; }

        public IEnumerable<Produto> Produtos { get; set; }

    }
}
