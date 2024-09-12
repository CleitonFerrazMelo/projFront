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
        public Regra BuscarRegraPorUsuario(IdentityUser nomeUsuario);
        public IdentityUser BuscarUsuarioPorID(string id);
        public List<IdentityRole> BuscarRegras();
        public Regra BuscarRegraPorNome(string nomeRegra);
        public void LimparRegraUsuario(string id);
        public void CadastrarRegraNoUsuario(string userId, string roleId);
    }
}
