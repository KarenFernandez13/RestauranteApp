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
    public class MesaController : Controller
    {
        private readonly RestauranteContext _context;

        public MesaController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: Mesa
        public async Task<IActionResult> Index(DateTime? fecha)
        {
            if (!fecha.HasValue)
            {
                fecha = DateTime.Today;
            }

            var mesas = await _context.Mesas.Include(m => m.IdSucursalNavigation).ToListAsync();
            var reservasDelDia = await _context.Reservas
                .Where(r => r.Fecha == DateOnly.FromDateTime(fecha.Value))
                .ToListAsync();

            foreach (var mesa in mesas)
            {
                var reserva = reservasDelDia.FirstOrDefault(r => r.IdMesa == mesa.Id);
                if (reserva != null)
                {
                    if (reserva.Estado == "Confirmada")
                        mesa.Estado = "Ocupada";
                    else if (reserva.Estado == "Pendiente")
                        mesa.Estado = "Reservada";
                    else
                        mesa.Estado = "Disponible";

                }
            }

            ViewData["Fecha"] = fecha.Value.ToString("yyyy-MM-dd");
            return View(mesas);
        }

        // Marcar mesa como ocupada
        [HttpPost]
        public async Task<IActionResult> MarcarOcupada(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }

            mesa.Estado = "Ocupada";
            _context.Update(mesa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Marcar mesa como disponible
        [HttpPost]
        public async Task<IActionResult> MarcarDisponible(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }

            mesa.Estado = "Disponible";
            _context.Update(mesa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Marcar mesa como reservada
        [HttpPost]
        public async Task<IActionResult> MarcarReservada(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }

            mesa.Estado = "Reservada";
            _context.Update(mesa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Mesa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas
                .Include(m => m.IdSucursalNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesa == null)
            {
                return NotFound();
            }

            return View(mesa);
        }

        // GET: Mesa/Create
        public IActionResult Create()
        {
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id");
            return View();
        }

        // POST: Mesa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero,Capacidad,Estado,IdSucursal")] Mesa mesa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mesa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id", mesa.IdSucursal);
            return View(mesa);
        }

        // GET: Mesa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id", mesa.IdSucursal);
            return View(mesa);
        }

        // POST: Mesa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero,Capacidad,Estado,IdSucursal")] Mesa mesa)
        {
            if (id != mesa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mesa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MesaExists(mesa.Id))
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
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id", mesa.IdSucursal);
            return View(mesa);
        }

        // GET: Mesa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas
                .Include(m => m.IdSucursalNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesa == null)
            {
                return NotFound();
            }

            return View(mesa);
        }

        // POST: Mesa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa != null)
            {
                _context.Mesas.Remove(mesa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MesaExists(int id)
        {
            return _context.Mesas.Any(e => e.Id == id);
        }
    }
}
