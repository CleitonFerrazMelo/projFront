using projFront.Models;
using projFront.Repository;

namespace projFront.Services
{
    public class NotaFiscalServices : INotaFiscalServices
    {
        public readonly INotaFiscalRepository _notaFiscalRepository;

        public NotaFiscalServices(INotaFiscalRepository notaFiscalRepository)
        {
            _notaFiscalRepository = notaFiscalRepository;
        }

        public string ValidarDelecao(NotaFiscal notaFiscal)
        {
            string mensagem = string.Empty;
            try
            {
                if (notaFiscal.FaturaNumero > 0)
                    mensagem = "Nota Fiscal não pode ser excluida porque já foi impressa";

                if (string.IsNullOrEmpty(mensagem))
                    _notaFiscalRepository.Deletar(notaFiscal);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
            return mensagem;
        }

        public NotaFiscal? RetornarUltimaNota(string cnpj)
        {
            NotaFiscal? notaFiscal = _notaFiscalRepository.LocalizarUltima(cnpj);
            return notaFiscal;
        }
    }
}
