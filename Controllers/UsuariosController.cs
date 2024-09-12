using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projFront.Data;
using projFront.Models;
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

        public UsuariosController(AppDbContext context, IUsuarioServices usuarioServices, IMapper mapper)
        {
            _context = context;
            _usuarioServices = usuarioServices;
            _mapper = mapper;
        }


        // GET: Usuarios
        public IActionResult Index()
        {
            ViewData["PaginaSelecionada"] = "Usuarios";

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UsuarioViewModel usuarioVM)
        {
            if (id != usuarioVM.Id)
            {
                return NotFound();
            }



            if (ModelState.IsValid)
            {
               
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
    }
}
