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
    [Authorize(Policy = "CanVerPermisosRoles")]
    public class PermisoController : Controller
    {
        private readonly RestauranteContext _context;

        public PermisoController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: Permiso
        public async Task<IActionResult> Index()
        {
            var restauranteContext = _context.Permisos.Include(p => p.Roles);
            return View(await restauranteContext.ToListAsync());
        }

        // GET: Permiso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(m => m.Numero == id);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // GET: Permiso/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id");
            return View();
        }

        // POST: Permiso/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numero,Descripcion,IdRol")] Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id", permiso.Roles);
            return View(permiso);
        }

        // GET: Permiso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id", permiso.Roles);
            return View(permiso);
        }

        // POST: Permiso/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Numero,Descripcion,IdRol")] Permiso permiso)
        {
            if (id != permiso.Numero)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermisoExists(permiso.Numero))
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
            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id", permiso.Roles);
            return View(permiso);
        }

        // GET: Permiso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(m => m.Numero == id);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // POST: Permiso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso != null)
            {
                _context.Permisos.Remove(permiso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.Numero == id);
        }
    }
}
