using projFront.Models;
using projFront.Repository;

namespace projFront.Services
{
    public class BancoServices: IBancoServices
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

        public string ValidarDelecao(Banco banco)
        {
            string mensagem = string.Empty;
            if (_bancoRepository.RelacionadoNotaFiscal(banco.IdBanco))
                mensagem = "Banco não pode ser deletado porque está relacionado a Nota Fiscal";

            if (string.IsNullOrEmpty(mensagem))
                _bancoRepository.Deletar(banco);

            return mensagem;
        }
    }
}
