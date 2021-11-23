using Domain.Base;

namespace Domain.DTO
{
    public class Produto 
    {
        public int Id_Produto { get; set; }

        public string Nome { get; set; }
        public string ImagemUrl { get; set; }
        public decimal Preco { get; set; }
        public int Id_Categoria { get; set; }

        
    }
}
