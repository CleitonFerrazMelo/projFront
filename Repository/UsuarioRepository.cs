using projFront.Data;
using projFront.Models;

namespace projFront.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly AppDbContext _repo;

        public UsuarioRepository(AppDbContext repo)
        {
            _repo = repo;
        }

        public void DeletarUsuario(Usuario usuario)
        {
            _repo.Remove(usuario);
            _repo.SaveChanges();
        }
    }
}
