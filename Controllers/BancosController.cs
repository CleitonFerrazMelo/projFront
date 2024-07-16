using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projFront.Data;
using projFront.Models;
using projFront.Services;
using projFront.ViewModels;
using projFront.ViewModels.Mappings;

namespace projFront.Controllers
{
    public class BancosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBancoServices _bancoServices;
        private readonly IMapper _mapper;

        public BancosController(AppDbContext context, IBancoServices bancoServices, IMapper mapper)
        {
            _context = context;
            _bancoServices = bancoServices;
            _mapper = mapper;
        }


        // GET: Bancos
        public async Task<IActionResult> Index()
        {
            ViewData["PaginaSelecionada"] = "Bancos";

            IEnumerable<BancoViewModel> listaBancoViewModel = null;

            if (_context.Bancos != null)
            {
                var listaBancos = await _context.Bancos.ToListAsync();
                listaBancoViewModel = _mapper.Map<IEnumerable<BancoViewModel>>(listaBancos);
            }          

            return View(listaBancoViewModel);
        }

        // GET: Bancos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bancos == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos
                .FirstOrDefaultAsync(m => m.Id == id);
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
            return View();
        }

        // POST: Bancos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Agencia,TipoConta,NumeroConta,PixChave,PixNumero")] Banco banco)
        {
            if (ModelState.IsValid)
            {
                _context.Add(banco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banco);
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
            var bancoViewModel = _mapper.Map<BancoViewModel>(banco);

            return View(bancoViewModel);
        }

        // POST: Bancos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Agencia,TipoConta,NumeroConta,PixChave,PixNumero")] Banco banco)
        {
            if (id != banco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BancoExists(banco.Id))
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
            return View(banco);
        }

        // GET: Bancos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bancos == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos
                .FirstOrDefaultAsync(m => m.Id == id);
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
          return (_context.Bancos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
