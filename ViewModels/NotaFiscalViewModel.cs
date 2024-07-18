using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class NotaFiscalViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe Nome Cliente!")]
        public string Nome { get; set; }
        
        public string Cnpj { get; set; }
        [Display(Name = "Inscrição Estadual")]
        public string IncricaoEstadual { get; set; }
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }
        [Display(Name = "Número")]
        public string Numero { get; set; }
        
        public string Bairro { get; set; }
        [Display(Name = "Nome da cidade")]
        public string NomeCidade { get; set; }
        [Display(Name = "UF")]
        public string Uf { get; set; }
        [Display(Name = "CEP")]
        public string Cep { get; set; }
        [Display(Name = "Número do Telefone")]
        public string NumeroTelefone { get; set; }
        
        [Display(Name = "Descrição do Serviço")]
        [Required(ErrorMessage = "Informe a Descrição do Serviço!")]
        public string DescricaoServico { get; set; }
        
        [Display(Name = "Valor Total")]
        [Required(ErrorMessage = "Informe o valor do Serviço!")]
        public decimal ValorTotal { get; set; }

        public string Banco { get; set; }
        [Display(Name = "Agência")]
        public string Agencia { get; set; }
        
        public string Conta { get; set; }
        [Display(Name = "Chave PIX")]
        public string PixChave { get; set; }
        [Display(Name = "Número do PIX")]
        public string PixNumero { get; set; }
        
        public int IdEmpresa { get; set; }
        [Display(Name = "Data de Emissão")]
        public DateTime? DataEmissao { get; set; }
        [Display(Name = "Série")]
        public string FaturaSerie { get; set; }
        [Display(Name = "Número da Fatura")]
        public int FaturaNumero { get; set; }
        [Display(Name = "Mensagem do Fisco")]
        public string MensagemFisco { get; set; }
    }
}
