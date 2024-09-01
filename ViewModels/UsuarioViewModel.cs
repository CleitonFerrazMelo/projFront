﻿using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class UsuarioViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Informe nome Usuário!")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Direito { get; set; }

    }
}
