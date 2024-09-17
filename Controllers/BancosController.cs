using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projFront.Data;
using projFront.Migrations;
using projFront.Models;
using projFront.Repository;
using projFront.Services;
using projFront.ViewModels;
using projFront.ViewModels.Mappings;

namespace projFront.Controllers
{
    [Authorize(Roles = "Admin,Operador")]
    public class BancosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBancoServices _bancoServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsuarioRepository _IUsuarioRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public BancosController(AppDbContext context, IBancoServices bancoServices, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUsuarioRepository iUsuarioRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _bancoServices = bancoServices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _IUsuarioRepository = iUsuarioRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        // GET: Bancos
        public async Task<IActionResult> Index()
        {
            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            //var listaUsuarios = _IUsuarioRepository.ListarTodosOsUsuariosAsync();

            ViewData["UsuarioLogado"] = usuarioLogado;
            ViewData["PaginaSelecionada"] = "Bancos";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();

            ApplicationUser userLogado = _IUsuarioRepository.BuscarUserPorEmail(usuarioLogado);
            Regra direitoUsuarioLogado = _IUsuarioRepository.BuscarRegraPorUsuario(userLogado);

            IEnumerable<BancoViewModel> listaBancoViewModel = null;
            List<Banco> listaBancoPorUsuario = null;
            IEnumerable<BancoViewModel> listaBancoPorUsuarioVM = null;

            if (_context.Bancos != null)
            {
                var listaBancos = await _context.Bancos.ToListAsync();
                listaBancoViewModel = _mapper.Map<IEnumerable<BancoViewModel>>(listaBancos);
            }

            if(direitoUsuarioLogado.Nome == "Admin")
            {
                return View(listaBancoViewModel);
            }

            ApplicationUser dadosUsuario = _IUsuarioRepository.BuscarUserPorEmail(usuarioLogado);

            listaBancoPorUsuario = _bancoServices.ListaBancosPorUsuario(dadosUsuario.Id);
            listaBancoPorUsuarioVM = _mapper.Map<IEnumerable<BancoViewModel>>(listaBancoPorUsuario);

            return View(listaBancoPorUsuarioVM);
        }

        // GET: Bancos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bancos == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos
                .FirstOrDefaultAsync(m => m.IdBanco == id);
            if (banco == null)
            {
                return NotFound();
            }
            var bancoViewModel = _mapper.Map<BancoViewModel>(banco);

            return View(bancoViewModel);
        }

        // GET: Bancos/Create
        public IActionResult Create()
        {
            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            ApplicationUser userLogado = _IUsuarioRepository.BuscarUserPorEmail(usuarioLogado);

            Regra direitoUsuarioLogado = _IUsuarioRepository.BuscarRegraPorUsuario(userLogado);

            var listaUsuarios = _IUsuarioRepository.ListarTodosOsUsuariosAsync();
            
            ViewData["usuarioLogado"] = usuarioLogado;
            ViewData["direitoUsuarioLogado"] = direitoUsuarioLogado.Nome;

            BancoViewModel bancoViewModel = new BancoViewModel();
            bancoViewModel.ListaUsuariosNaoRelacionados.AddRange(listaUsuarios);

            return View(bancoViewModel);
        }

        // POST: Bancos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(BancoViewModel bancoVM)
        {
            if (ModelState.IsValid)
            {
                Banco banco = _mapper.Map<Banco>(bancoVM);
                _context.Add(banco);
                _context.SaveChanges();

                Banco bancoSalvo = _context.Bancos.FirstOrDefault(b => b.Nome == banco.Nome 
                                                                    && b.PixNumero == banco.PixNumero
                                                                    && b.Agencia == banco.Agencia
                                                                    && b.TipoConta == banco.TipoConta
                                                                    && b.NumeroConta == banco.NumeroConta
                                                                    && b.PixChave == banco.PixChave);
                
                int idBanco = bancoSalvo.IdBanco;

                List<string> listaUsuariosId = new List<string>();

                foreach (var usuario in bancoVM.ListaUsuariosRelacionados)
                {
                    var usuarios = _IUsuarioRepository.BuscarUserPorEmail(usuario.Email);

                    listaUsuariosId.Add(usuarios.Id);
                }

                ValidaIncluiUsuario(listaUsuariosId, idBanco);

                return RedirectToAction("Index");
            }
            
            return View(bancoVM);
        }

        private void ValidaIncluiUsuario(List<string> listIdUsuario, int idBanco)
        {
            _bancoServices.ValidaIncluiUsuario(listIdUsuario, idBanco);
        }
        private void LimpaEIncluiUsuario(List<string> listIdUsuario, int idBanco)
        {
            _bancoServices.LimpaEIncluiUsuario(listIdUsuario, idBanco);
        }

        // GET: Bancos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bancos == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos.FindAsync(id);
            if (banco == null)
            {
                return NotFound();
            }

            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            ApplicationUser userLogado = _IUsuarioRepository.BuscarUserPorEmail(usuarioLogado);

            Regra direitoUsuarioLogado = _IUsuarioRepository.BuscarRegraPorUsuario(userLogado);

            ViewData["direitoUsuarioLogado"] = direitoUsuarioLogado.Nome;

            List<ApplicationUser> listaUsuariosRelacionadosPorBanco = _IUsuarioRepository.BuscarUserPorBanco(banco.IdBanco);
            List<ApplicationUser> listaUsuarioNaoRelacionadosPorBanco = _IUsuarioRepository.ListarTodosOsUsuariosAsync();

            foreach (var usuarioRelacionado in listaUsuariosRelacionadosPorBanco)
            {
                listaUsuarioNaoRelacionadosPorBanco.Remove(usuarioRelacionado);
            }

            var bancoViewModel = _mapper.Map<BancoViewModel>(banco);

            bancoViewModel.ListaUsuariosRelacionados = listaUsuariosRelacionadosPorBanco;
            bancoViewModel.ListaUsuariosNaoRelacionados = listaUsuarioNaoRelacionadosPorBanco;

            return View(bancoViewModel);
        }

        // POST: Bancos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> Edit(int id, BancoViewModel bancoVM)
        {
            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            ApplicationUser userLogado = _IUsuarioRepository.BuscarUserPorEmail(usuarioLogado);

            Regra direitoUsuarioLogado = _IUsuarioRepository.BuscarRegraPorUsuario(userLogado);

            if (id != bancoVM.IdBanco)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Banco banco = _mapper.Map<Banco>(bancoVM);
                try
                {
                    _context.Update(banco);
                    await _context.SaveChangesAsync();

                    if(direitoUsuarioLogado.Nome == "Admin")
                    {
                        Banco bancoSalvo = _context.Bancos.FirstOrDefault(b => b.Nome == banco.Nome
                                                                    && b.PixNumero == banco.PixNumero
                                                                    && b.Agencia == banco.Agencia
                                                                    && b.TipoConta == banco.TipoConta
                                                                    && b.NumeroConta == banco.NumeroConta
                                                                    && b.PixChave == banco.PixChave);

                        int idBanco = bancoSalvo.IdBanco;

                        List<string> listaUsuariosId = new List<string>();

                        foreach (var usuario in bancoVM.ListaUsuariosRelacionados)
                        {
                            var usuarios = _IUsuarioRepository.BuscarUserPorEmail(usuario.Email);

                            listaUsuariosId.Add(usuarios.Id);
                        }

                        LimpaEIncluiUsuario(listaUsuariosId, idBanco);
                    }                    

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BancoExists(banco.IdBanco))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bancoVM);
        }

        // GET: Bancos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bancos == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos
                .FirstOrDefaultAsync(m => m.IdBanco == id);
            if (banco == null)
            {
                return NotFound();
            }

            var bancoViewModel = _mapper.Map<BancoViewModel>(banco);

            return View(bancoViewModel);
        }

        // POST: Bancos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bancos == null)
            {
                return Problem("Entity set 'AppDbContext.Bancos'  is null.");
            }
            var banco = await _context.Bancos.FindAsync(id);
            if (banco != null)
            {
                string mensagem = _bancoServices.ValidarDelecao(banco);
                if (!string.IsNullOrEmpty(mensagem))
                    return Problem(mensagem);
                //_context.Bancos.Remove(banco);
            }
            
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BancoExists(int id)
        {
          return (_context.Bancos?.Any(e => e.IdBanco == id)).GetValueOrDefault();
        }
        private string IdentificaDireitoUsuario()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string direito = "Operador";

            if (userId != null)
            {
                var userRoles = _userManager.GetRolesAsync(_userManager.FindByIdAsync(userId).GetAwaiter().GetResult());

                direito = userRoles.Result[0];
            }

            return direito;
        }
    }
}
