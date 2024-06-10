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
    public class ReservaController : Controller
    {
        private readonly RestauranteContext _context;

        public ReservaController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: Reserva
        public async Task<IActionResult> Index()
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
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "Id", "Id");
            return View();
        }

        // POST: Reserva/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CiCliente,IdMesa,Fecha,Estado,Hora")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CiCliente"] = new SelectList(_context.Clientes, "Ci", "Ci", reserva.CiCliente);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "Id", "Id", reserva.IdMesa);
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
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "Id", "Id", reserva.IdMesa);
            return View(reserva);
        }

        // POST: Reserva/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CiCliente,IdMesa,Fecha,Estado,Hora")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
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
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "Id", "Id", reserva.IdMesa);
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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
