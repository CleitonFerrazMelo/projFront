using projFront.Models;

namespace projFront.Repository
{
    public interface INotaFiscalRepository
    {
        public void Deletar(NotaFiscal notaFiscal);

        public NotaFiscal LocalizarUltima(string cnpj);
        public List<NotaFiscal> RetornaListaNotaFiscal(string email);

        public Empresa RetornaEmpresa(int id);

        public void AtualizarUltimoNumeroEmpresa(Empresa empresa);

        public void Atualizar(NotaFiscal notaFiscal);
    }
}
