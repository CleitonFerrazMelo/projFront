using Microsoft.AspNetCore.Identity;
using projFront.Models;

namespace projFront.Repository
{
    public interface IUsuarioRepository
    {
        public void Deletar(Usuario usuario);
        public List<IdentityUser> ListarTodosOsUsuariosAsync();
        public IdentityUser BuscarUserPorEmail(string email);
        public List<IdentityUser> BuscarUserPorBanco(int IdBanco);
    }
}
