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
            if (_empresaRepository.RelacionadoNotaFiscal(empresa.IdEmpresa))
                mensagem = "Empresa não pode ser deletado porque está relacionado a Nota Fiscal";

            if (string.IsNullOrEmpty(mensagem))
                _empresaRepository.Deletar(empresa);

            return mensagem;
        }

        public List<Empresa> GetEmpresas()
        {
            return _empresaRepository.GetEmpresas();
        }
    }
}
