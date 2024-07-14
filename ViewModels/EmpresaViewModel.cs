using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class EmpresaViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe nome Empresa!")]
        public string Nome { get; set; }
        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Informe o CNPJ Empresa!")]
        public string Cnpj { get; set; }

        [Display(Name = "IE")]
        [Required(ErrorMessage = "Informe a IE Empresa!")]
        public string Ie { get; set; }

        [Display(Name = "Endereço")]
        [Required(ErrorMessage = "Informe o endereço da Empresa!")]
        public string Endereco { get; set; }
        
        [Display(Name = "Número")]
        [Required(ErrorMessage = "Informe o número da Empresa!")]
        public string Numero { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "Informe o bairro da Empresa!")]
        public string Bairro { get; set; }

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "Informe o nome cidade da Empresa!")]
        public string NomeCidade { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "Informe o CEP da Empresa!")]
        public string Cep { get; set; }

        [Display(Name = "Série")]
        [Required(ErrorMessage = "Informe a serie para gerar as notas fiscais!")]
        public string NotaFiscalSerie { get; set; }

        [Display(Name = "Número última nota")]
        [Required(ErrorMessage = "Informe o número da ultima nota fiscal!")]
        public string FaturaUltimoNumero { get; set; }

        [Display(Name = "Mensagem da Nota Fiscal")]
        [Required(ErrorMessage = "Informe mensagem para ser exibida nota fiscal!")]
        public string MensagemFisco { get; set; }
        public string FaturaSerie { get; set; }

    }
}
