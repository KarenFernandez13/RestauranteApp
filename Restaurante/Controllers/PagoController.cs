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
        private readonly CurrencyLayerService _currencyLayerService;

        public PagoController(RestauranteContext context, CurrencyLayerService currencyLayerService)
        {
            _context = context;
            _currencyLayerService = currencyLayerService;
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

        // GET: Pagos/Create
        public async Task<IActionResult> Create(int? idOrden)
        {
            if (idOrden == null)
            {
                return NotFound();
            }

            var orden = _context.Orden
                .Include(o => o.OrdenDetalles)
                .Include(o => o.IdReservaNavigation)
                .ThenInclude(r => r.CiClienteNavigation)
                .FirstOrDefault(o => o.Id == idOrden);

            if (orden == null)
            {
                return NotFound();
            }      
            
            var pago = new Pago
            {
                Monto = (double)orden.Total,
                Moneda = "-",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                IdOrden = idOrden,
                IdOrdenNavigation = orden            
                                
            };

            if (pago == null)
            {
                return NotFound();
            }

            ViewBag.Monedas = new SelectList(new List<string> { "USD", "EUR", "UYU" });
            ViewBag.Metodos = new SelectList(new List<string> { "Efectivo", "Tarjeta" });
            ViewBag.IdOrden = idOrden;
            ViewBag.DocumentoCliente = orden.IdReservaNavigation.CiClienteNavigation.Ci;

            return View(pago);
        }
      

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Monto,Moneda,Fecha,Metodo,IdOrden")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var montoString = pago.Monto.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    pago.Monto = double.Parse(montoString, System.Globalization.CultureInfo.InvariantCulture);

                    _context.Add(pago);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbEx)
                {
                    var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                    ModelState.AddModelError("", $"Error al convertir el monto: {innerException}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                }
            }

            ViewBag.Monedas = new SelectList(new List<string> { "UYU", "USD", "EUR" });
            ViewBag.Metodos = new SelectList(new List<string> { "Efectivo", "Tarjeta" });

            return View(pago);
        }


        // Método para obtener el tipo de cambio
        [HttpGet]
        public async Task<IActionResult> GetExchangeRates(string moneda)
        {
            var rates = await _currencyLayerService.GetExchangeRatesAsync("UYU", new List<string> { "USD", "EUR" });
            if (rates.TryGetValue(moneda, out var rate))
            {
                return Json(rate);
            }
            return Json(0);
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
            ViewData["IdOrden"] = new SelectList(_context.Orden, "Id", "Id", pago.IdOrden);
            return View(pago);
        }

        // POST: Pago/Edit/5
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
            ViewData["IdOrden"] = new SelectList(_context.Orden, "Id", "Id", pago.IdOrden);
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
