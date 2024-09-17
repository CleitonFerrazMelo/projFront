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

        public List<NotaFiscal> RetornaListaNotaFiscal(string email)
        {
            List<NotaFiscal> listaNotaFiscal = _notaFiscalRepository.RetornaListaNotaFiscal(email);
            return listaNotaFiscal;
        }

        public string Imprimir(NotaFiscal notaFiscal)
        {
            string mensagem = string.Empty;
            if (notaFiscal.IdEmpresa == 0)
                mensagem = "Falta informar empresa";
            
            if (notaFiscal.FaturaNumero == 0)
            {
                Empresa empresa = RetornaEmpresa(notaFiscal.IdEmpresa);
                ApplicationUser usuario = RetornaProximoNumero(notaFiscal.UserName);
                if (empresa.IdEmpresa > 0)
                {
                    notaFiscal.FaturaSerie = empresa.FaturaSerie;
                    //notaFiscal.FaturaNumero = empresa.FaturaUltimoNumero;
                    notaFiscal.FaturaNumero = usuario.NumeroDaNotaAtual;

                    notaFiscal.DataEmissao = DateTime.Now;
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

        private Empresa RetornaEmpresa(int idEmpresa)
        {
            Empresa empresa = _notaFiscalRepository.RetornaEmpresa(idEmpresa);
            if (empresa.IdEmpresa > 0)
            {
                empresa.FaturaUltimoNumero = empresa.FaturaUltimoNumero + 1;
                //_notaFiscalRepository.AtualizarUltimoNumeroEmpresa(empresa);
            }

            return empresa;
        }

        private ApplicationUser RetornaProximoNumero(string email)
        {
            ApplicationUser usuario = _notaFiscalRepository.RetornaUsuario(email);
            if (usuario.Id != null)
            {
                usuario.NumeroDaNotaAtual = usuario.NumeroDaNotaAtual + 1;
                _notaFiscalRepository.AtualizarUltimoNumeroUsuario(usuario);
            }
            return usuario;
        }
    }
}
