using System.ComponentModel.DataAnnotations;

namespace projFront.Models
{
    public class Banco
    {
        [Key]
        public int IdBanco { get; set; }
        public string Nome { get; set; }
        public string Agencia { get; set; }
        public string TipoConta { get; set; }
        public string NumeroConta { get; set; }
        public string PixChave { get; set; }
        public string PixNumero { get; set; }
    }
}


