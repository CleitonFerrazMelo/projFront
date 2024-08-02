using projFront.Models;

namespace projFront.Services
{
    public interface IEmpresaServices
    {
        public string ValidarDelecao(Empresa empresa);

        public List<Empresa> GetEmpresas();

        public List<Empresa> GetEmpresa(int id);
    }
}
