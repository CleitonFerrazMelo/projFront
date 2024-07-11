using System.ComponentModel.DataAnnotations;

namespace projFront.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
    }

}
