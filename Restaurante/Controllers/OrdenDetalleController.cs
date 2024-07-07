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
    public class OrdenDetalleController : Controller
    {
        private readonly RestauranteContext _context;

        public OrdenDetalleController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: OrdenDetalle
        public async Task<IActionResult> Index()
        {
            var restauranteContext = _context.OrdenDetalles.Include(o => o.CodigoProdNavigation).Include(o => o.IdOrdenNavigation);
            return View(await restauranteContext.ToListAsync());
        }


        // GET: OrdenDetalle/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var orden = await _context.Orden
            .Include(o => o.OrdenDetalles)
                .ThenInclude(od => od.CodigoProdNavigation)
            .Include(o => o.IdReservaNavigation)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }


        // GET: OrdenDetalle/Create
        public IActionResult Create(int idOrden)
        {
            var tiposProductos = _context.Productos
                .Select(p => p.Tipo)
                .Distinct()
                .ToList();

            ViewBag.TiposProductos = tiposProductos.Select(t => new SelectListItem { Value = t, Text = t }).ToList();
            ViewBag.IdOrden = idOrden;

            return View(new OrdenDetalle { IdOrden = idOrden });
        }


        // POST: OrdenDetalle/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int idOrden, string tipoProducto, string[] productos)
        {
            if (productos != null && productos.Length > 0)
            {
                var orden = await _context.Orden.Include(o => o.OrdenDetalles)
                                                  .FirstOrDefaultAsync(o => o.Id == idOrden);
                if (orden == null)
                {
                    return NotFound();
                }

                foreach (var prod in productos)
                {
                    var partes = prod.Split(':');
                    if (partes.Length == 2 && int.TryParse(partes[0], out int codigoProd) && int.TryParse(partes[1], out int cantidad))
                    {
                        var ordenDetalle = new OrdenDetalle
                        {
                            IdOrden = idOrden,
                            CodigoProd = codigoProd,
                            Cantidad = cantidad
                        };
                        orden.OrdenDetalles.Add(ordenDetalle);
                        _context.Add(ordenDetalle);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = idOrden });
            }

            var tiposProductos = _context.Productos
                .Select(p => p.Tipo)
                .Distinct()
                .ToList();

            ViewBag.TiposProductos = tiposProductos.Select(t => new SelectListItem { Value = t, Text = t }).ToList();
            ViewBag.IdOrden = idOrden;
            ViewBag.Productos = _context.Productos
                .Where(p => p.Tipo == tipoProducto)
                .Select(p => new { p.Codigo, p.Nombre })
                .ToList();

            return View(new OrdenDetalle { IdOrden = idOrden });
        }

        [HttpGet]
        public JsonResult GetProductosByTipo(string tipo)
        {
            var productos = _context.Productos.Where(p => p.Tipo == tipo).Select(p => new { Codigo = p.Codigo, Nombre = p.Nombre }).ToList();

            return Json(productos);
        }

        // GET: OrdenDetalle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenDetalle = await _context.OrdenDetalles.FindAsync(id);
            if (ordenDetalle == null)
            {
                return NotFound();
            }
            ViewData["CodigoProd"] = new SelectList(_context.Productos, "Codigo", "Codigo", ordenDetalle.CodigoProd);
            ViewData["IdOrden"] = new SelectList(_context.Orden, "Id", "Id", ordenDetalle.IdOrden);
            return View(ordenDetalle);
        }

        // POST: OrdenDetalle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cantidad,IdOrden,CodigoProd")] OrdenDetalle ordenDetalle)
        {
            if (id != ordenDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordenDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenDetalleExists(ordenDetalle.Id))
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
            ViewData["CodigoProd"] = new SelectList(_context.Productos, "Codigo", "Codigo", ordenDetalle.CodigoProd);
            ViewData["IdOrden"] = new SelectList(_context.Orden, "Id", "Id", ordenDetalle.IdOrden);
            return View(ordenDetalle);
        }

        // GET: OrdenDetalle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenDetalle = await _context.OrdenDetalles
                .Include(o => o.CodigoProdNavigation)
                .Include(o => o.IdOrdenNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordenDetalle == null)
            {
                return NotFound();
            }

            return View(ordenDetalle);
        }

        // POST: OrdenDetalle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordenDetalle = await _context.OrdenDetalles.FindAsync(id);
            if (ordenDetalle != null)
            {
                _context.OrdenDetalles.Remove(ordenDetalle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenDetalleExists(int id)
        {
            return _context.OrdenDetalles.Any(e => e.Id == id);
        }

        public async Task<IActionResult> OrdenDetalle()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }
    }
}
