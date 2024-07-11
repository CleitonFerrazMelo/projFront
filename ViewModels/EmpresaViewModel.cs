using System.ComponentModel.DataAnnotations;

namespace projFront.ViewModels
{
    public class EmpresaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe nome Empresa!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o CNPJ Empresa!")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "Informe a IE Empresa!")]
        public string Ie { get; set; }

        [Required(ErrorMessage = "Informe o endereço da Empresa!")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Informe o número da Empresa!")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Informe o bairro da Empresa!")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Informe o nome cidade da Empresa!")]
        public string NomeCidade { get; set; }

        [Required(ErrorMessage = "Informe o CEP da Empresa!")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Informe a serie para gerar as notas fiscais!")]
        public string NotaFiscalSerie { get; set; }

        [Required(ErrorMessage = "Informe o número da ultima nota fiscal!")]
        public string NotaFiscalUltimoNumero { get; set; }

        [Required(ErrorMessage = "Informe mensagem para ser exibida nota fiscal!")]
        public string MensagemFisco { get; set; }
        
    }
}
