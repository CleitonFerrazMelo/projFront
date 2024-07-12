using projFront.Models;
using projFront.Repository;

namespace projFront.Services
{
    public class EmpresaServices: IEmpresaServices
    {
        private readonly IEmpresaRepository _empresaRepository;

        public EmpresaServices(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        public string ValidarDelecao(Empresa empresa)
        {
            string mensagem = string.Empty;

            _empresaRepository.Deletar(empresa);

            return mensagem;
        }
    }
}
