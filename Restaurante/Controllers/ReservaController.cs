using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Models;

namespace Restaurante.Controllers
{
    [Authorize(Policy = "CanVerReservas")]
    public class ReservaController : Controller
    {
        private readonly RestauranteContext _context;

        public ReservaController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: Reserva

        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var reservas = _context.Reservas
                           .Include(r => r.IdMesaNavigation)
                           .Include(r => r.CiClienteNavigation)
                           .AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                var start = DateOnly.FromDateTime(startDate.Value);
                var end = DateOnly.FromDateTime(endDate.Value);
                reservas = reservas.Where(r => r.Fecha >= start && r.Fecha <= end);
            }
            else if (startDate.HasValue)
            {
                var start = DateOnly.FromDateTime(startDate.Value);
                reservas = reservas.Where(r => r.Fecha >= start);
            }
            else if (endDate.HasValue)
            {
                var end = DateOnly.FromDateTime(endDate.Value);
                reservas = reservas.Where(r => r.Fecha <= end);
            }
            else
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                reservas = reservas.Where(r => r.Fecha >= today);
            }
            reservas = reservas.OrderBy(r => r.Fecha);
            return View(reservas.ToList());
        }


        public async Task<IActionResult> ListaReservas()
        {
            var restauranteContext = _context.Reservas.Include(r => r.CiClienteNavigation).Include(r => r.IdMesaNavigation);
            return View(await restauranteContext.ToListAsync());
        }


        // GET: Reserva/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.CiClienteNavigation)
                .Include(r => r.IdMesaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reserva/Create
        public IActionResult Create()
        {
            ViewData["CiCliente"] = new SelectList(_context.Clientes, "Ci", "Ci");
            ViewData["IdMesa"] = new SelectList(_context.Mesas.Where(m => m.Estado == "Disponible"), "Id", "Numero");
            return View();
        }

        // POST: Reserva/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CiCliente,Fecha,Hora,IdMesa,Estado")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                var mesa = await _context.Mesas.FindAsync(reserva.IdMesa);
                if (mesa != null)
                {
                    if (reserva.Estado == "Pendiente")
                    {
                        mesa.Estado = "Reservada";

                    }
                    else if (reserva.Estado == "Confirmada")
                    {
                        mesa.Estado = "Ocupada";
                    }
                    else if (reserva.Estado == "Cancelada")
                    {
                        mesa.Estado = "Disponible";
                    }
                    _context.Update(mesa);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CiCliente"] = new SelectList(_context.Clientes, "Ci", "Ci", reserva.CiCliente);
            ViewData["IdMesa"] = new SelectList(_context.Mesas.Where(m => m.Estado == "Disponible"), "Id", "Numero", reserva.IdMesa);
            return View(reserva);
        }

        // GET: Reserva/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["CiCliente"] = new SelectList(_context.Clientes, "Ci", "Ci", reserva.CiCliente);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "Id", "Numero", reserva.IdMesa);
            return View(reserva);
        }

        // POST: Reserva/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CiCliente,Fecha,Hora,IdMesa,Estado")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mesaAnteriorId = _context.Reservas.AsNoTracking().FirstOrDefault(r => r.Id == id)?.IdMesa;

                    _context.Update(reserva);
                    await _context.SaveChangesAsync();

                    // Actualizar estado de la mesa anterior a Disponible
                    if (mesaAnteriorId.HasValue && mesaAnteriorId != reserva.IdMesa)
                    {
                        var mesaAnterior = await _context.Mesas.FindAsync(mesaAnteriorId.Value);
                        if (mesaAnterior != null)
                        {
                            mesaAnterior.Estado = "Disponible";
                            _context.Update(mesaAnterior);
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Actualizar estado de la nueva mesa a Ocupada
                    var nuevaMesa = await _context.Mesas.FindAsync(reserva.IdMesa);
                    if (nuevaMesa != null)
                    {
                        nuevaMesa.Estado = "Ocupada";
                        _context.Update(nuevaMesa);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            ViewData["CiCliente"] = new SelectList(_context.Clientes, "Ci", "Ci", reserva.CiCliente);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "Id", "Numero", reserva.IdMesa);
            return View(reserva);
        }

        // GET: Reserva/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.CiClienteNavigation)
                .Include(r => r.IdMesaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reserva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);

                // Actualizar estado de la mesa a Disponible
                var mesa = await _context.Mesas.FindAsync(reserva.IdMesa);
                if (mesa != null)
                {
                    mesa.Estado = "Disponible";
                    _context.Update(mesa);
                    await _context.SaveChangesAsync();
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }


        [HttpPost]
        public IActionResult UpdateStatus(int id, string estado)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
            {
                return NotFound();
            }

            reserva.Estado = estado;
            _context.SaveChanges();

            return Json(new { success = true });
        }



    }
}