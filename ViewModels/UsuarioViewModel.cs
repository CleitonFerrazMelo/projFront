using projFront.Models;
using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class UsuarioViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Informe nome Usuário!")]
        [Display(Name = "Username do Usuário")]
        public string UserName { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        public List<Regra> Direito { get; set; } = new();
        public int NumeroDaNotaAtual { get; set; } = 0;
        public int UltimoNumeroDaNota { get; set; } = 0;

    }
}
