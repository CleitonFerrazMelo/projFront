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

        public string ValidarDelecao(Banco banco)
        {
            string mensagem = string.Empty;
            if (_bancoRepository.RelacionadoNotaFiscal(banco.Nome))
                mensagem = "Banco não pode ser deletado porque está relacionado a Nota Fiscal";

            if (string.IsNullOrEmpty(mensagem))
                _bancoRepository.Deletar(banco);

            return mensagem;
        }
    }
}
