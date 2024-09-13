using projFront.Models;
using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class NotaFiscalViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo Nome do cliente é obrigatório.")]
        public string Nome { get; set; }
        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "O campo CNPJ é obrigatório.")]
        public string Cnpj { get; set; }

        [Display(Name = "Inscrição Estadual")]
        [Required(ErrorMessage = "O campo Inscrição Estadual é obrigatório.")]
        public string Ie { get; set; }

        [Display(Name = "Endereço")]
        [Required(ErrorMessage = "O campo Endereço é obrigatório.")]
        public string Endereco { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "O campo Número é obrigatório.")]
        public string Numero { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
        public string Bairro { get; set; }

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        public string NomeCidade { get; set; }

        [Display(Name = "UF")]
        [Required(ErrorMessage = "O campo Unidade Federação é obrigatório.")]
        public string Uf { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "O campo CEP é obrigatório.")]
        public string Cep { get; set; }

        [Display(Name = "Número do Telefone")]
        [Required(ErrorMessage = "O campo Unidade Federação é obrigatório.")]
        public string NumeroTelefone { get; set; }
        
        [Display(Name = "Descrição do Serviço")]
        [Required(ErrorMessage = "O campo Descrição do Serviço é obrigatório.")]
        //[Required(ErrorMessage = "Informe a Descrição do Serviço!")]
        public string DescricaoServico { get; set; }
        
        [Display(Name = "Valor Total")]
        [Required(ErrorMessage = "O campo Valor Total é obrigatório.")]
        //[Required(ErrorMessage = "Informe o valor do Serviço!")]
        public string ValorTotal { get; set; }

        public int IdBanco { get; set; }
        public List<Banco> Banco { get; set; }

        [Display(Name = "Agência")]
        public string Agencia { get; set; }
        
        public string Conta { get; set; }
       
        [Display(Name = "Chave PIX")]
        public string PixChave { get; set; }

        [Display(Name = "Número do PIX")]
        public string PixNumero { get; set; }
        
        public string IdEmpresa { get; set; }
        public List<Empresa> Empresa { get; set; }

        [Display(Name = "Data de Emissão")]
        public DateTime? DataEmissao { get; set; }

        [Display(Name = "Série")]
        public string FaturaSerie { get; set; }

        [Display(Name = "Número da Fatura")]
        public int FaturaNumero { get; set; }

        [Display(Name = "Mensagem do Fisco")]
        [Required(ErrorMessage = "O campo Mensagem do Fisco é obrigatório.")]
        public string MensagemFisco { get; set; }
        public string UserName { get; set; }
    }
}
