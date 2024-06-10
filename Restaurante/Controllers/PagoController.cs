using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Models;

namespace Restaurante.Controllers
{
    public class PagoController : Controller
    {
        private readonly RestauranteContext _context;

        public PagoController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: Pago
        public async Task<IActionResult> Index()
        {
            var restauranteContext = _context.Pagos.Include(p => p.IdClimaNavigation).Include(p => p.IdCotizacionNavigation).Include(p => p.IdOrdenNavigation);
            return View(await restauranteContext.ToListAsync());
        }

        // GET: Pago/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.IdClimaNavigation)
                .Include(p => p.IdCotizacionNavigation)
                .Include(p => p.IdOrdenNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pago/Create
        public IActionResult Create()
        {
            ViewData["IdClima"] = new SelectList(_context.Climas, "Id", "Id");
            ViewData["IdCotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id");
            ViewData["IdOrden"] = new SelectList(_context.Ordens, "Id", "Id");
            return View();
        }

        // POST: Pago/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Monto,Moneda,Fecha,Metodo,IdOrden,IdClima,TotalConDescuento,Descuento,IdCotizacion")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdClima"] = new SelectList(_context.Climas, "Id", "Id", pago.IdClima);
            ViewData["IdCotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id", pago.IdCotizacion);
            ViewData["IdOrden"] = new SelectList(_context.Ordens, "Id", "Id", pago.IdOrden);
            return View(pago);
        }

        // GET: Pago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["IdClima"] = new SelectList(_context.Climas, "Id", "Id", pago.IdClima);
            ViewData["IdCotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id", pago.IdCotizacion);
            ViewData["IdOrden"] = new SelectList(_context.Ordens, "Id", "Id", pago.IdOrden);
            return View(pago);
        }

        // POST: Pago/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Monto,Moneda,Fecha,Metodo,IdOrden,IdClima,TotalConDescuento,Descuento,IdCotizacion")] Pago pago)
        {
            if (id != pago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id))
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
            ViewData["IdClima"] = new SelectList(_context.Climas, "Id", "Id", pago.IdClima);
            ViewData["IdCotizacion"] = new SelectList(_context.Cotizacions, "Id", "Id", pago.IdCotizacion);
            ViewData["IdOrden"] = new SelectList(_context.Ordens, "Id", "Id", pago.IdOrden);
            return View(pago);
        }

        // GET: Pago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.IdClimaNavigation)
                .Include(p => p.IdCotizacionNavigation)
                .Include(p => p.IdOrdenNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }
    }
}
