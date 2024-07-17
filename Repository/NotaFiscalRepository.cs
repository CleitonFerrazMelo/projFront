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

        public NotaFiscal LocalizarUltima(string cnpj)
        {
            NotaFiscal notaFiscal = _repo.NotaFiscal.Where(x => x.Cnpj == cnpj).First();
            if (notaFiscal == null)
                notaFiscal= new NotaFiscal();
            return notaFiscal;
        }

        public Empresa RetornaEmpresa(int id)
        {
            Empresa empresa = _repo.Empresas.Where(x => x.Id == id).First();
            if (empresa == null)
                empresa = new Empresa();
            return empresa;
        }

        public void AtualizarUltimoNumeroEmpresa(Empresa empresa)
        {
            _repo.Update(empresa);
            _repo.SaveChanges();
        }

        public void Atualizar(NotaFiscal notaFiscal)
        {
            _repo.Update(notaFiscal);
            _repo.SaveChanges();
        }
    }
}
