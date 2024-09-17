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

        public UsuarioViewModel BuscarUsuarioPorID(string id)
        {
            ApplicationUser usuario = _usuarioRepository.BuscarUsuarioPorID(id);
            Regra regra = _usuarioRepository.BuscarRegraPorUsuario(usuario);
            UsuarioViewModel usuarioVM = new UsuarioViewModel();
            usuarioVM.Id = usuario.Id;
            usuarioVM.UserName = usuario.UserName;
            usuarioVM.Email = usuario.Email;
            usuarioVM.NumeroDaNotaAtual = usuario.NumeroDaNotaAtual;
            usuarioVM.UltimoNumeroDaNota = usuario.UltimoNumeroDaNota;

            usuarioVM.Direito.Add(regra);

            return usuarioVM;
        }

        public List<UsuarioViewModel> ListarTodosUsuarios()
        {
            List<ApplicationUser> listaUsuarios = _usuarioRepository.ListarTodosOsUsuariosAsync();

            var listaUsuarioViewModel = MapearUsuarioToUsuarioVM(listaUsuarios);

            return listaUsuarioViewModel;
        }

        public List<UsuarioViewModel> MapearUsuarioToUsuarioVM(List<ApplicationUser> listaUsuarios)
        {
            List<UsuarioViewModel> listaUsuarioVM = new List<UsuarioViewModel>();            

            foreach (var usuario in listaUsuarios)
            {
                UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
                usuarioViewModel.Id = usuario.Id;
                usuarioViewModel.UserName = usuario.UserName;
                usuarioViewModel.Email = usuario.Email;
                Regra regra = new Regra();
                regra = _usuarioRepository.BuscarRegraPorUsuario(usuario);
                usuarioViewModel.Direito.Add(regra);    

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

        public List<Regra> CarregarListaRegras()
        {
            List<IdentityRole> listIdentityRoles = _usuarioRepository.BuscarRegras();
            List<Regra> listaRegras = new List<Regra>();

            foreach (var rules in listIdentityRoles)
            {
                Regra regra = new Regra();
                regra.IdRegra = rules.Id;
                regra.Nome = rules.Name;
                listaRegras.Add(regra);
            }

            return listaRegras;
        }

        public Regra GetRegra(string nomeRegra)
        {
            return _usuarioRepository.BuscarRegraPorNome(nomeRegra);
        }

        public string AlterarRegraNoUsuario(UsuarioViewModel usuarioVM)
        {
            string mensagem = string.Empty;
            ApplicationUser usuario = _usuarioRepository.BuscarUsuarioPorID(usuarioVM.Id);

            if (_usuarioRepository.ValidarNumeroNotaFiscal(usuario))
            {
                mensagem = "Já exsite Usuário com essa faixa de Número de Nota Fiscal";
                throw new Exception(mensagem);
            }
            else
            {
                usuario.NumeroDaNotaAtual = usuarioVM.NumeroDaNotaAtual;
                usuario.UltimoNumeroDaNota = usuarioVM.UltimoNumeroDaNota;

                _usuarioRepository.AlterarDadosNota(usuario);

                _usuarioRepository.LimparRegraUsuario(usuarioVM.Id);

                _usuarioRepository.CadastrarRegraNoUsuario(usuarioVM.Id, usuarioVM.Direito[0].IdRegra);
            }
            return mensagem;
        }

    }
}
