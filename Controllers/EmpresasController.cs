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

        public EmpresasController(AppDbContext context, IEmpresaServices empresaServices, IMapper mapper)
        {
            _context = context;
            _empresaServices = empresaServices;
            _mapper = mapper;
        }


        // GET: Empresas
        public async Task<IActionResult> Index()
        {
            ViewData["PaginaSelecionada"] = "Empresas";

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
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Telefone,Cnpj,Ie,Endereco,Numero,Bairro,NomeCidade,UF,Cep,FaturaSerie,FaturaUltimoNumero,MensagemFisco")] EmpresaViewModel empresaVM)
        {
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
            if (_context.Empresas == null)
            {
                return Problem("Entity set 'AppDbContext.Empresas'  is null.");
            }
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                string mensagem  = _empresaServices.ValidarDelecao(empresa);
                if (!string.IsNullOrEmpty(mensagem))
                    return Problem(mensagem);
                //_context.Empresas.Remove(empresa);
            }
            
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
          return (_context.Empresas?.Any(e => e.IdEmpresa == id)).GetValueOrDefault();
        }
    }
}
