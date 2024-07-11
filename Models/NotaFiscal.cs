using System.ComponentModel.DataAnnotations;

namespace projFront.Models
{
    public class NotaFiscal
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string IncricaoEstadual { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string NomeCidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public string NumeroTelefone { get; set; }
        public string DescricaoServico { get; set; }
        public decimal ValorTotal { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string PixChave { get; set; }
        public string PixNumero { get; set; }
        public int IdEmpresa { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string FaturaSerie { get; set; }
        public int FaturaNumero { get; set; }
        public string MensagemFisco { get; set; }
    }
}


