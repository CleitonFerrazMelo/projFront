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

            _bancoRepository.Deletar(banco);

            return mensagem;
        }
    }
}
