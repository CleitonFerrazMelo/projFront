using Microsoft.AspNetCore.Identity;
using projFront.Data;
using projFront.Models;

namespace projFront.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly AppDbContext _repo;
        private readonly UserManager<IdentityUser> _userManager;
        public UsuarioRepository(AppDbContext repo, UserManager<IdentityUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public void Deletar(Usuario usuario)
        {
            _repo.Remove(usuario);
            _repo.SaveChanges();
        }

        public IdentityUser BuscarUserPorEmail(string email)
        {
            return _userManager.FindByEmailAsync(email).Result;
        }
        public List<IdentityUser> ListarTodosOsUsuariosAsync()
        {
            return _userManager.Users.ToList();
        }
    }
}
