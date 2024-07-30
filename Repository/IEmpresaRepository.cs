using projFront.Models;

namespace projFront.Repository
{
    public interface IEmpresaRepository
    {
        public void Deletar(Empresa empresa);

        public bool RelacionadoNotaFiscal(int idEmpresa);

        public List<Empresa> GetEmpresas();
    }
}
