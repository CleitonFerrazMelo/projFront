using projFront.Models;

namespace projFront.Repository
{
    public interface INotaFiscalRepository
    {
        public void Deletar(NotaFiscal notaFiscal);

        public NotaFiscal? LocalizarUltima(string cnpj);
    }
}
