using projFront.Models;
using projFront.ViewModels;

namespace projFront.Services
{
    public interface IUsuarioServices
    {
        public string ValidarDelecao(Usuario usuario);
        public List<UsuarioViewModel> ListarTodosUsuarios();
        
    }
}
