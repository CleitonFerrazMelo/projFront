using System.ComponentModel.DataAnnotations;

namespace projFront.Models
{
    public class Empresa
    {
        [Key]
        public int IdEmpresa { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Ie { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string NomeCidade { get; set; }
        public string UF { get; set; }
        public string Cep { get; set; }
        public string FaturaSerie { get; set; }
        public int FaturaUltimoNumero { get; set; }
        public string MensagemFisco { get; set; }
    }
}

