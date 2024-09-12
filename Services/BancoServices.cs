using projFront.Models;
using projFront.Repository;

namespace projFront.Services
{
    public class BancoServices : IBancoServices
    {
        private readonly IBancoRepository _bancoRepository;

        public BancoServices(IBancoRepository bancoRepository)
        {
            _bancoRepository = bancoRepository;
        }

        public List<Banco> GetBancos()
        {
            return _bancoRepository.GetBancos();
        }
        public List<Banco> GetBanco(int id)
        {
            Banco banco = _bancoRepository.GetBanco(id);
            if(banco != null)
            {
                List<Banco> listaBanco = new List<Banco>();
                listaBanco.Add(banco);
                return listaBanco;
            }
            
            return null;
        }

        public string ValidarDelecao(Banco banco)
        {
            string mensagem = string.Empty;
            if (_bancoRepository.RelacionadoNotaFiscal(banco.IdBanco))
                mensagem = "Banco não pode ser deletado porque está relacionado a Nota Fiscal";

            if (string.IsNullOrEmpty(mensagem))
                _bancoRepository.Deletar(banco);

            return mensagem;
        }

        public void ValidaIncluiUsuario(List<string> listIdUsuario, int idBanco)
        {
            _bancoRepository.ValidaIncluiUsuario(listIdUsuario, idBanco);
        }
        public void LimpaEIncluiUsuario(List<string> listIdUsuario, int idBanco)
        {
            _bancoRepository.LimpaEIncluiUsuario(listIdUsuario, idBanco);
        }

        public List<Banco> ListaBancosPorUsuario(string idUsuario)
        {
            return _bancoRepository.ListaBancosPorUsuario(idUsuario);
        }
    }
}
