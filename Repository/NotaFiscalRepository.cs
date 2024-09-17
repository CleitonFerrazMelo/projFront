using Microsoft.AspNetCore.Identity;
using projFront.Data;
using projFront.Models;
using System.Collections.ObjectModel;

namespace projFront.Repository
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        public readonly AppDbContext _repo;
        public readonly UserManager<ApplicationUser> _userManager;

        public NotaFiscalRepository(AppDbContext repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public void Deletar(NotaFiscal notaFiscal)
        {
            _repo.Remove(notaFiscal);
            _repo.SaveChanges();
        }

        public NotaFiscal LocalizarUltima(string cnpj)
        {
            NotaFiscal notaFiscal = _repo.NotaFiscal.Where(x => x.Cnpj == cnpj).FirstOrDefault();
            if (notaFiscal == null)
                notaFiscal= new NotaFiscal();
            return notaFiscal;
        }

        public Empresa RetornaEmpresa(int id)
        {
            Empresa empresa = _repo.Empresas.Where(x => x.IdEmpresa == id).First();
            if (empresa == null)
                empresa = new Empresa();
            return empresa;
        }
        public ApplicationUser RetornaUsuario(string email)
        {
            ApplicationUser usuario = _userManager.Users.Where(x => x.Email== email).First();
            if (usuario == null)
                usuario = new ApplicationUser();
            return usuario;
        }

        public List<NotaFiscal> RetornaListaNotaFiscal(string email)
        {
            List<NotaFiscal> listaNotaFiscal = _repo.NotaFiscal.Where(x => x.UserName == email).ToList();
            if (listaNotaFiscal == null)
                listaNotaFiscal = new List<NotaFiscal>();
            return listaNotaFiscal;
        }

        public void AtualizarUltimoNumeroEmpresa(Empresa empresa)
        {
            _repo.Update(empresa);
            _repo.SaveChanges();
        }

        public void AtualizarUltimoNumeroUsuario(ApplicationUser usuario)
        {
            _repo.Update(usuario);
            _repo.SaveChanges();
        }

        public void Atualizar(NotaFiscal notaFiscal)
        {
            _repo.Update(notaFiscal);
            _repo.SaveChanges();
        }
    }
}
