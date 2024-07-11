using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class BancoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe nome Banco!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe número agencia!")]
        public string Agencia { get; set; }

        [Required(ErrorMessage = "Informe tipo de conta!")]
        public string TipoConta { get; set; }

        [Required(ErrorMessage = "Informe número da conta!")]
        public string NumeroConta { get; set; }

        [Required(ErrorMessage = "Informe chave do PIX!")]
        public string PixChave { get; set; }

        [Required(ErrorMessage = "Informe número do PIX!")]
        public string PixNumero { get; set; }
    }
}
