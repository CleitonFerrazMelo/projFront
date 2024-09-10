using projFront.Models;

namespace projFront.Services
{
    public interface INotaFiscalServices
    {
        public string ValidarDelecao(NotaFiscal notaFiscal);

        public NotaFiscal RetornarUltimaNota(string cnpj);

        public List<NotaFiscal> RetornaListaNotaFiscal(string email);

        public string Imprimir(NotaFiscal notaFiscal);
    }
}
