using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class BancoViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome do Banco")]
        [Required(ErrorMessage = "Informe nome Banco!")]
        public string Nome { get; set; }

        [Display(Name = "Número da agência")]
        [Required(ErrorMessage = "Informe número agencia!")]
        public string Agencia { get; set; }

        [Display(Name = "Tipo da Conta")]
        [Required(ErrorMessage = "Informe tipo de conta!")]
        public string TipoConta { get; set; }

        [Display(Name = "Número da Conta")]
        [Required(ErrorMessage = "Informe número da conta!")]
        public string NumeroConta { get; set; }

        [Display(Name = "Chave do Pix")]
        [Required(ErrorMessage = "Informe chave do PIX!")]
        public string PixChave { get; set; }

        [Display(Name = "Número do Pix")]
        [Required(ErrorMessage = "Informe número do PIX!")]
        public string PixNumero { get; set; }
    }
}
