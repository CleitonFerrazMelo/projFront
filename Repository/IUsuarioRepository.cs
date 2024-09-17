using Microsoft.AspNetCore.Identity;
using projFront.Models;

namespace projFront.Repository
{
    public interface IUsuarioRepository
    {
        public void Deletar(Usuario usuario);
        public List<ApplicationUser> ListarTodosOsUsuariosAsync();
        public ApplicationUser BuscarUserPorEmail(string email);
        public List<ApplicationUser> BuscarUserPorBanco(int IdBanco);
        public Regra BuscarRegraPorUsuario(ApplicationUser nomeUsuario);
        public ApplicationUser BuscarUsuarioPorID(string id);
        public List<IdentityRole> BuscarRegras();
        public Regra BuscarRegraPorNome(string nomeRegra);
        public void LimparRegraUsuario(string id);
        public void CadastrarRegraNoUsuario(string userId, string roleId);
        public void AlterarDadosNota(ApplicationUser usuario);
        public bool ValidarNumeroNotaFiscal(ApplicationUser usuario);
    }
}
