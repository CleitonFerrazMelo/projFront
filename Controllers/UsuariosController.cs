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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsuariosController(AppDbContext context, IUsuarioServices usuarioServices, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _usuarioServices = usuarioServices;
            _mapper = mapper;
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
        public async Task<IActionResult> Edit(string id, UsuarioViewModel usuarioVM)
        {
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
               

                if (!string.IsNullOrEmpty(mensagem))
                    return Problem(mensagem);
                return RedirectToAction(nameof(Index));
            }
            return View(usuarioVM);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'AppDbContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                string mensagem = _usuarioServices.ValidarDelecao(usuario);
                if (!string.IsNullOrEmpty(mensagem))
                    return Problem(mensagem);
                //_context.Usuarios.Remove(usuario);
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
    }
}
