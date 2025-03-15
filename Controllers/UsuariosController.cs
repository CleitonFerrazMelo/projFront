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
using projFront.Models;
using projFront.Models.Exception;
using projFront.Repository;
using projFront.Services;
using projFront.ViewModels;

namespace projFront.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioServices _usuarioServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsuariosController(AppDbContext context, IUsuarioServices usuarioServices, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _usuarioServices = usuarioServices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        // GET: Usuarios
        public IActionResult Index()
        {
            ViewData["PaginaSelecionada"] = "Usuarios";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();

            var listaUsuarioVM = _usuarioServices.ListarTodosUsuarios();

            return _context.Usuarios != null ? 
                          View(_usuarioServices.ListarTodosUsuarios()) :
                          Problem("Entity set 'AppDbContext.Usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["PaginaSelecionada"] = "Usuarios";
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Senha")] UsuarioViewModel usuarioVM)
        {
            ViewData["PaginaSelecionada"] = "Usuarios";
            if (ModelState.IsValid)
            {
                Usuario usuario = _mapper.Map<Usuario>(usuarioVM); 
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarioVM);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["PaginaSelecionada"] = "Usuarios";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            UsuarioViewModel usuario = _usuarioServices.BuscarUsuarioPorID(Convert.ToString(id));

            Regra regraSelecionada = _usuarioServices.GetRegra(usuario.Direito[0].Nome);

            ViewBag.DireitoSelecionado = regraSelecionada;

            usuario.Direito = _usuarioServices.CarregarListaRegras();

            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> Edit(string id,[FromBody] UsuarioViewModel usuarioVM)
        {
            ViewData["PaginaSelecionada"] = "Usuarios";
            if (id != usuarioVM.Id)
            {
                return NotFound();
            }
            string mensagem = "";

            if (ModelState.IsValid)
            {
                try
                {
                    mensagem = _usuarioServices.AlterarRegraNoUsuario(usuarioVM);
                }
                catch (NumeroNotaInvalidoException ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }


                if(!string.IsNullOrEmpty(mensagem))
                {
                    TempData["Mensagem"] = mensagem;
                    return View(usuarioVM);
                }

                TempData["Mensagem"] = "Usuário salvo com sucesso!";
                return Ok(new { success = true, message = "Usuário salvo com sucesso!" });

            }
            return View(usuarioVM);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["PaginaSelecionada"] = "Usuarios";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            UsuarioViewModel usuario = _usuarioServices.BuscarUsuarioPorID(Convert.ToString(id));

            Regra regraSelecionada = _usuarioServices.GetRegra(usuario.Direito[0].Nome);

            ViewBag.DireitoSelecionado = regraSelecionada;

            usuario.Direito = _usuarioServices.CarregarListaRegras();

            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'AppDbContext.Usuarios'  is null.");
            }
            UsuarioViewModel usuarioVM = _usuarioServices.BuscarUsuarioPorID(Convert.ToString(id));

            var usuario = await _userManager.FindByEmailAsync(usuarioVM.Email);

            if (usuario != null)
            {
                var resultado = await _userManager.DeleteAsync(usuario);
                if (resultado.Succeeded)
                {
                    TempData["Mensagem"] = "Usuário excluído com sucesso!";
                    return RedirectToAction("Index", "Usuarios");
                }
                else
                {
                    TempData["Mensagem"] = "Usuário possui relacionamentos. " + resultado.Errors;
                    return View(usuarioVM);
                }
            }
            
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
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

        [AllowAnonymous]
        public async Task<IActionResult> AlterarSenha(Guid? id)
        {
            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            //var listaUsuarios = _IUsuarioRepository.ListarTodosOsUsuariosAsync();

            ViewData["UsuarioLogado"] = usuarioLogado;
            ViewData["PaginaSelecionada"] = "alterarSenha";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();

            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            string idString = Convert.ToString(id);

            var usuario = _userManager.Users.Where(x => x.Id == idString).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioVM = new UsuarioSenhaViewModel
            {
                Id = usuario.Id,
                UserName = usuario.UserName,
                // Adicione outras propriedades conforme necessário
            };

            return View("AlterarSenhaUsuario", usuarioVM);
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> AlterarNovaSenha(string id, UsuarioSenhaViewModel usuarioVM)
        {
            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            //var listaUsuarios = _IUsuarioRepository.ListarTodosOsUsuariosAsync();

            ViewData["UsuarioLogado"] = usuarioLogado;
            ViewData["PaginaSelecionada"] = "alterarSenha";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id != usuarioVM.Id || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }

            // Lógica para alterar a senha
            usuario.PasswordHash = _userManager.PasswordHasher.HashPassword(usuario, usuarioVM.NewPassword); // Exemplo de alteração de senha
            _context.Update(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Senha alterada com sucesso." });
        }

    }
}
