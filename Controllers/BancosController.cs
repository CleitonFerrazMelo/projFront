﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projFront.Data;
using projFront.Models;
using projFront.Services;

namespace projFront.Controllers
{
    public class BancosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBancoServices _bancoServices;

        public BancosController(AppDbContext context, IBancoServices bancoServices)
        {
            _context = context;
            _bancoServices = bancoServices;
        }


        // GET: Bancos
        public async Task<IActionResult> Index()
        {
              return _context.Bancos != null ? 
                          View(await _context.Bancos.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Bancos'  is null.");
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

            return View(banco);
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
            return View(banco);
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

            return View(banco);
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
                _bancoServices.ValidarDelecao(banco);
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
