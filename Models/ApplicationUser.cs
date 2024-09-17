using Microsoft.AspNetCore.Identity;

namespace projFront.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int NumeroDaNotaAtual { get; set; }
        public int UltimoNumeroDaNota { get; set; }
       
    }
}
