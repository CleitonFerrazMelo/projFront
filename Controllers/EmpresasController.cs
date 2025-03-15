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
using projFront.Services;
using projFront.ViewModels;

namespace projFront.Controllers
{
    [Authorize(Roles = "Admin, Operador")]
    public class EmpresasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmpresaServices _empresaServices;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public EmpresasController(AppDbContext context, IEmpresaServices empresaServices, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _empresaServices = empresaServices;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        // GET: Empresas
        public async Task<IActionResult> Index()
        {
            ViewData["PaginaSelecionada"] = "Empresas";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();

            IEnumerable<EmpresaViewModel> listaEmpresaViewModel = null;

            if (_context.Empresas != null)
            {
                var listaEmpresas = await _context.Empresas.ToListAsync();
                listaEmpresaViewModel = _mapper.Map<IEnumerable<EmpresaViewModel>>(listaEmpresas);
            }

            return View(listaEmpresaViewModel);
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["PaginaSelecionada"] = "Empresas";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.IdEmpresa == id);
            if (empresa == null)
            {
                return NotFound();
            }
            EmpresaViewModel empresaVM = _mapper.Map<EmpresaViewModel>(empresa);
            return View(empresaVM);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            ViewData["PaginaSelecionada"] = "Empresas";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Telefone,Cnpj,Ie,Endereco,Numero,Bairro,NomeCidade,UF,Cep,FaturaSerie,FaturaUltimoNumero,MensagemFisco")] EmpresaViewModel empresaVM)
        {
            ViewData["PaginaSelecionada"] = "Empresas";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (ModelState.IsValid)
            {
                Empresa empresa = _mapper.Map<Empresa>(empresaVM);
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(empresaVM);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["PaginaSelecionada"] = "Empresas";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            var empresaViewModel = _mapper.Map<EmpresaViewModel>(empresa);

            return View(empresaViewModel);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmpresaViewModel empresaVM)
        {
            ViewData["PaginaSelecionada"] = "Empresas";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id != empresaVM.IdEmpresa)
            {
                return NotFound();
            }

            var empresa = _mapper.Map<Empresa>(empresaVM);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.IdEmpresa))
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

            return View(empresaVM);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["PaginaSelecionada"] = "Empresas";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.IdEmpresa == id);
            if (empresa == null)
            {
                return NotFound();
            }
            EmpresaViewModel empresaVM = _mapper.Map<EmpresaViewModel>(empresa);
            return View(empresaVM);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewData["PaginaSelecionada"] = "Empresas";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (_context.Empresas == null)
            {
                return Problem("Entity set 'AppDbContext.Empresas'  is null.");
            }
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                string mensagem  = _empresaServices.ValidarDelecao(empresa);
                if (!string.IsNullOrEmpty(mensagem))
                {
                    EmpresaViewModel empresaVM = _mapper.Map<EmpresaViewModel>(empresa);
                    TempData["Mensagem"] = mensagem;
                    return View(empresaVM);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
          return (_context.Empresas?.Any(e => e.IdEmpresa == id)).GetValueOrDefault();
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
