using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe nome Usuário!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe a senha Usuário!")]
        public string Senha { get; set; }

    }
}
