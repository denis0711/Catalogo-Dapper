using Domain.Base;

namespace Domain.Entities
{
    public class Autenticacao
    {
        public string Login { get; set; }
        public byte[] Password { get; set; }
        public int? CodigoUsuario { get; set; }
        public string Nome { get; set; }
        public bool? Ativo { get; set; }
        public bool? AlterarSenha { get; set; }
        public int? CodigoConta { get; set; }
        public int CodigoPerfil { get; set; }
        public string DescricaoPerfil { get; set; }
        public bool? Lider { get; set; }
    }
}
