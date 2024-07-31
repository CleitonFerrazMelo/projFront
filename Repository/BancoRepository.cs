using projFront.Data;
using projFront.Models;

namespace projFront.Repository
{
    public class BancoRepository: IBancoRepository
    {
        public readonly AppDbContext _repo;
        private readonly NotaFiscal _notaFiscal;
        public BancoRepository(AppDbContext repo)
        {
            _repo = repo;
        }
        public void Deletar(Banco banco)
        {
            _repo.Remove(banco);
            _repo.SaveChanges();
        }

        public bool RelacionadoNotaFiscal(int id)
        {
            return _repo.NotaFiscal.Where(x => x.IdBanco == id).Any();
        }

        public List<Banco> GetBancos()
        {
            return _repo.Bancos.ToList();
        }
    }
}
