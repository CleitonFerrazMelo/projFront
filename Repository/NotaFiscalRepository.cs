using projFront.Data;
using projFront.Models;

namespace projFront.Repository
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        public readonly AppDbContext _repo;

        public NotaFiscalRepository(AppDbContext repo)
        {
            _repo = repo;
        }

        public void Deletar(NotaFiscal notaFiscal)
        {
            _repo.Remove(notaFiscal);
            _repo.SaveChanges();
        }

        public NotaFiscal? LocalizarUltima(string cnpj)
        {
            NotaFiscal? notaFiscal = _repo.NotaFiscal.Where(x => x.Cnpj == cnpj).FirstOrDefault();
            return notaFiscal;
        }
    }
}
