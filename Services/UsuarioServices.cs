using Microsoft.AspNetCore.Identity;
using projFront.Models;
using projFront.Repository;
using projFront.ViewModels;

namespace projFront.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        public readonly IUsuarioRepository _usuarioRepository;

        public UsuarioServices(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public List<UsuarioViewModel> ListarTodosUsuarios()
        {
            List<IdentityUser> listaUsuarios = _usuarioRepository.ListarTodosOsUsuariosAsync();

            var listaUsuarioViewModel = MapearUsuarioToUsuarioVM(listaUsuarios);

            return listaUsuarioViewModel;
        }

        public List<UsuarioViewModel> MapearUsuarioToUsuarioVM(List<IdentityUser> listaUsuarios)
        {
            List<UsuarioViewModel> listaUsuarioVM = new List<UsuarioViewModel>();            

            foreach (var usuario in listaUsuarios)
            {
                UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
                usuarioViewModel.Id = usuario.Id;
                usuarioViewModel.UserName = usuario.UserName;
                usuarioViewModel.Email = usuario.Email;

                _usuarioRepository.BuscarRegraPorUsuario(usuario);


                listaUsuarioVM.Add(usuarioViewModel);
            }
            return listaUsuarioVM;
        }

        public string ValidarDelecao(Usuario  usuario)
        {
            string mensagem = string.Empty;

            _usuarioRepository.Deletar(usuario);
            return mensagem;
        }
    }
}
