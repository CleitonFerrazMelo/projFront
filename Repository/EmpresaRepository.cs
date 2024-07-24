using projFront.Data;
using projFront.Models;

namespace projFront.Repository
{
    public class EmpresaRepository: IEmpresaRepository
    {
        public readonly AppDbContext _repo;
        private readonly NotaFiscal _notaFiscal;

        public EmpresaRepository(AppDbContext repo)
        {
            _repo = repo;
        }
        public void Deletar(Empresa empresa)
        {
            _repo.Remove(empresa);
            _repo.SaveChanges();
        }

        public bool RelacionadoNotaFiscal(int idEmpresa)
        {
            return _repo.NotaFiscal.Where(x => x.IdEmpresa == idEmpresa).Any();
        }
    }
}
