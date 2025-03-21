﻿using Microsoft.AspNetCore.Identity;
using projFront.Models;
using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class BancoViewModel
    {
        public int IdBanco { get; set; }

        [Display(Name = "Nome do Banco")]
        [Required(ErrorMessage = "Informe o nome do Banco!")]
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
        public string PixChave { get; set; } = string.Empty;

        [Display(Name = "Número do Pix")]
        public string PixNumero { get; set; } = string.Empty;
        public List<ApplicationUser> ListaUsuariosRelacionados { get; set; } = new List<ApplicationUser>();
        public List<ApplicationUser> ListaUsuariosNaoRelacionados { get; set; } = new List<ApplicationUser>();
    }
}
