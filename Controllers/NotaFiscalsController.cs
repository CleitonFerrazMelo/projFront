using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
using Rotativa.AspNetCore;

namespace projFront.Controllers
{
    [Authorize(Roles = "admin")]
    public class NotaFiscalsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly INotaFiscalServices _notaFiscalServices;
        private readonly IMapper _mapper;

        public NotaFiscalsController(AppDbContext context, INotaFiscalServices notaFiscalServices, IMapper mapper)
        {
            _context = context;
            _notaFiscalServices = notaFiscalServices;
            _mapper = mapper;
        }

        // GET: NotaFiscals
        public async Task<IActionResult> Index()
        {
            ViewData["PaginaSelecionada"] = "NotaFiscal";

            IEnumerable<NotaFiscalViewModel> listaNotaFiscalViewModel = null;

            if (_context.Empresas != null)
            {
                var listaNotaFiscal = await _context.NotaFiscal.ToListAsync();
                listaNotaFiscalViewModel = _mapper.Map<IEnumerable<NotaFiscalViewModel>>(listaNotaFiscal);
            }

            return View(listaNotaFiscalViewModel);
        }

        // GET: NotaFiscals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NotaFiscal == null)
            {
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaFiscal == null)
            {
                return NotFound();
            }
            NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);
            return View(notaFiscalVM);
        }

        // GET: NotaFiscals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotaFiscals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cnpj,IncricaoEstadual,Endereco,Numero,Bairro,NomeCidade,Uf,Cep,NumeroTelefone,DescricaoServico,ValorTotal,Banco,Agencia,Conta,PixChave,PixNumero,IdEmpresa,DataEmissao,FaturaSerie,FaturaNumero,MensagemFisco")] NotaFiscal notaFiscal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notaFiscal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);
            return View(notaFiscalVM);
        }

        // GET: NotaFiscals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NotaFiscal == null)
            {
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal.FindAsync(id);
            if (notaFiscal == null)
            {
                return NotFound();
            }
            NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);
            return View(notaFiscalVM);
        }

        // POST: NotaFiscals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cnpj,IncricaoEstadual,Endereco,Numero,Bairro,NomeCidade,Uf,Cep,NumeroTelefone,DescricaoServico,ValorTotal,Banco,Agencia,Conta,PixChave,PixNumero,IdEmpresa,DataEmissao,FaturaSerie,FaturaNumero,MensagemFisco")] NotaFiscal notaFiscal)
        {
            if (id != notaFiscal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notaFiscal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaFiscalExists(notaFiscal.Id))
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
            return View(notaFiscal);
        }

        // GET: NotaFiscals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NotaFiscal == null)
            {
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaFiscal == null)
            {
                return NotFound();
            }
            NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);
            return View(notaFiscalVM);
        }

        // POST: NotaFiscals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NotaFiscal == null)
            {
                return Problem("Entity set 'AppDbContext.NotaFiscal'  is null.");
            }
            var notaFiscal = await _context.NotaFiscal.FindAsync(id);
            if (notaFiscal != null)
            {
                string mensagem = _notaFiscalServices.ValidarDelecao(notaFiscal);
                if (!string.IsNullOrEmpty(mensagem))
                    return Problem(mensagem);
                //_context.NotaFiscal.Remove(notaFiscal);
            }
            
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: NotaFiscals/RetornarUltimaNota/5
        [HttpPost, ActionName("RetornarUltimaNota")]
        [ValidateAntiForgeryToken]
        public NotaFiscal? RetornarUltimaNota(string cnpj)
        {            
            var notaFiscal = _notaFiscalServices.RetornarUltimaNota(cnpj);            
            return notaFiscal;
        }

        // POST: NotaFiscals/Imprimir/5
        [HttpPost, ActionName("Imprimir")]
        [ValidateAntiForgeryToken]
        public string Imprimir(NotaFiscal notaFiscal)
        {
            return _notaFiscalServices.Imprimir(notaFiscal);
        }

        public async Task<ActionResult> ImprimePDF(int id)
        {
            if (id == null || _context.NotaFiscal == null)
            {
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal.FindAsync(id);
            if (notaFiscal == null)
            {
                return NotFound();
            }
            NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);
            return new ViewAsPdf("Impressao", notaFiscalVM) { FileName = "Test.pdf" };
        }

        private bool NotaFiscalExists(int id)
        {
          return (_context.NotaFiscal?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
