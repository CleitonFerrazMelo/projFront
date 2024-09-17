using Microsoft.AspNetCore.Identity;
using projFront.Models;
using projFront.ViewModels;

namespace projFront.Services
{
    public interface IUsuarioServices
    {
        public string ValidarDelecao(Usuario usuario);
        public List<UsuarioViewModel> ListarTodosUsuarios();
        public UsuarioViewModel BuscarUsuarioPorID(string id);
        public List<Regra> CarregarListaRegras();
        public Regra GetRegra(string nomeRegra);
        public string AlterarRegraNoUsuario(UsuarioViewModel usuarioVM);
    }
}
