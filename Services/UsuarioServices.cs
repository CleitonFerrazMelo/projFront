using projFront.Models;
using projFront.Repository;

namespace projFront.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        public readonly IUsuarioRepository _usuarioRepository;

        public UsuarioServices(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public string ValidarDelecao(Usuario  usuario)
        {
            string mensagem = string.Empty;

            _usuarioRepository.Deletar(usuario);
            return mensagem;
        }
    }
}
