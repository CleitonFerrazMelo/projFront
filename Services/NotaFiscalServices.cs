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

        public NotaFiscal RetornarUltimaNota(string cnpj)
        {
            NotaFiscal notaFiscal = _notaFiscalRepository.LocalizarUltima(cnpj);
            return notaFiscal;
        }

        public string Imprimir(NotaFiscal notaFiscal)
        {
            string mensagem = string.Empty;
            if (notaFiscal.IdEmpresa == 0)
                mensagem = "Falta informar empresa";
            
            if (notaFiscal.FaturaNumero == 0)
            {
                Empresa empresa = RetornaProximoNumero(notaFiscal.IdEmpresa);
                if (empresa.Id > 0)
                {
                    notaFiscal.FaturaSerie = empresa.FaturaSerie;
                    notaFiscal.FaturaNumero = empresa.FaturaUltimoNumero;
                }
                else
                {
                    mensagem = "Não foi possivel localizar empresa";
                }
            }

            if (notaFiscal.FaturaNumero > 0)
            {
                _notaFiscalRepository.Atualizar(notaFiscal);
            }

            return mensagem;
        }

        private Empresa RetornaProximoNumero(int idEmpresa)
        {
            Empresa empresa = _notaFiscalRepository.RetornaEmpresa(idEmpresa);
            if (empresa.Id > 0)
            {
                empresa.FaturaUltimoNumero = empresa.FaturaUltimoNumero + 1;
                _notaFiscalRepository.AtualizarUltimoNumeroEmpresa(empresa);
            }

            return empresa;
        }
    }
}
