using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Models;
using Microsoft.AspNetCore.Authorization;

namespace Restaurante.Controllers
{

    [Authorize(Policy = "CanVerOrdenes")]
    public class OrdenController : Controller
    {
        private readonly RestauranteContext _context;
        private readonly WeatherService _weatherService;

        public OrdenController(RestauranteContext context, WeatherService weatherService)
        {
            _context = context;
            _weatherService = weatherService;
        }

        private async Task UpdateOrdenTotalAsync(int idOrden)
        {
            var orden = await _context.Orden
                .Include(o => o.OrdenDetalles)
                .FirstOrDefaultAsync(o => o.Id == idOrden);

            if (orden != null)
            {
                orden.Total = orden.OrdenDetalles.Sum(od => od.Cantidad * od.CodigoProdNavigation.Precio);
                var clima = await _weatherService.GetTemperatureAsync();
                double descuento = CalculateDiscountPercentage(clima.Temperatura, clima.Descripcion);
                orden.Total -= orden.Total * descuento / 100;
            }
        }


        // GET: Orden
        public async Task<IActionResult> ListaOrdenes()
        {
            var restauranteContext = _context.Orden.Include(o => o.IdReservaNavigation).Include(o => o.IdUsuarioNavigation);
            return View(await restauranteContext.ToListAsync());
        }

        public async Task<IActionResult> Index()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var ordenes = _context.Orden
                                    .Include(o => o.IdReservaNavigation)
                                    .ThenInclude(r => r.IdMesaNavigation)
                                    .Include(o => o.IdReservaNavigation)
                                    .ThenInclude(r => r.CiClienteNavigation)
                                     .Where(o => o.IdReservaNavigation.Fecha == today)
                                        .ToList();
            return View(ordenes);
        }


        // GET: Orden/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var orden = await _context.Orden
                .Include(o => o.IdReservaNavigation)
                .ThenInclude(r => r.CiClienteNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .Include(o => o.OrdenDetalles)
                .ThenInclude(od => od.CodigoProdNavigation)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            var clima = await _weatherService.GetTemperatureAsync();
            var discountPercentage = CalculateDiscountPercentage(clima.Temperatura, clima.Descripcion);
            orden.Total = orden.OrdenDetalles.Sum(od => od.Cantidad * od.CodigoProdNavigation.Precio);
            var totalConDescuento = orden.Total - (orden.Total * (double)discountPercentage / 100);
            ViewBag.TotalConDescuento = totalConDescuento;
            ViewBag.discountPercentage = discountPercentage;
            return View(orden);
        }


        // GET: Orden/Create
        public async Task<IActionResult> Create(int idReserva)
        {
            var reserva = await _context.Reservas.FindAsync(idReserva);
            if (reserva == null)
            {
                return NotFound();
            }

            var clima = await _weatherService.GetTemperatureAsync();
            var discountPercentage = CalculateDiscountPercentage(clima.Temperatura, clima.Descripcion);

            var orden = new Orden
            {
                IdReserva = idReserva,
                Estado = "Activa",
                OrdenDetalles = new List<OrdenDetalle>()
            };

            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View(orden);
        }
        //public IActionResult Create(int idReserva)
        //{
        //    var reserva = _context.Reservas.Find(idReserva);
        //    if (reserva == null)
        //    {
        //        return NotFound();
        //    }

        //    var orden = new Orden
        //    {
        //        IdReserva = idReserva,
        //        Estado = "Activa",
        //        OrdenDetalles = new List<OrdenDetalle>()            Descuento = discountPercentage
        //    };         

        //    ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id");
        //    return View();

        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Total,Estado,IdUsuario,IdReserva")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orden);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", orden.IdUsuario);
            ViewData["Productos"] = new SelectList(_context.Productos, "Id", "Nombre");

            return View(orden);
        }


        // GET: Orden/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.Orden.FindAsync(id);
            if (orden == null)
            {
                return NotFound();
            }
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "Id", "Id", orden.IdReserva);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", orden.IdUsuario);
            return View(orden);
        }

        // POST: Orden/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Total,Estado,IdUsuario,IdReserva")] Orden orden)
        {
            if (id != orden.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orden);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenExists(orden.Id))
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
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "Id", "Id", orden.IdReserva);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", orden.IdUsuario);
            return View(orden);
        }

        // GET: Orden/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.Orden
                .Include(o => o.IdReservaNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        // POST: Orden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orden = await _context.Orden.FindAsync(id);
            if (orden != null)
            {
                _context.Orden.Remove(orden);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenExists(int id)
        {
            return _context.Orden.Any(e => e.Id == id);
        }


        public async Task<IActionResult> CheckOrder(int idReserva)
        {
            var ordenExistente = await _context.Orden.FirstOrDefaultAsync(o => o.IdReserva == idReserva);

            if (ordenExistente == null)
            {
                return RedirectToAction("Create", new { IdReserva = idReserva });
            }

            return RedirectToAction("Details", new { id = ordenExistente.Id });
        }


        [HttpPost]
        public IActionResult UpdateStatus(int id, string estado)
        {
            var orden = _context.Orden.Find(id);
            if (orden == null)
            {
                return NotFound();
            }

            orden.Estado = estado;
            _context.SaveChanges();

            return Json(new { success = true });
        }

        private double CalculateDiscountPercentage(double temperature, string description)
        {
            double discount = 0;

            if (temperature <= 10)
            {
                discount = 5;
            }
            else if (temperature < 0)
            {
                discount = 10;
            }

            if (description.Contains("rain", StringComparison.OrdinalIgnoreCase))
            {
                discount += 5;
            }

            return discount;
        }
    }
}

