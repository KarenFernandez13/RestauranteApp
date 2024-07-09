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

        

        // GET: Orden    
        public async Task<IActionResult> Index()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var ordenes = await _context.Orden
              .Include(o => o.IdReservaNavigation)
                  .ThenInclude(r => r.IdMesaNavigation)
              .Include(o => o.IdReservaNavigation)
                  .ThenInclude(r => r.CiClienteNavigation)
              .Include(o => o.Pagos) 
              .Where(o => o.IdReservaNavigation.Fecha == today)
              .ToListAsync();

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
                .Include(o => o.Pagos)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            // Obtener el tipo de cliente
            var tipoCliente = orden.IdReservaNavigation?.CiClienteNavigation?.TipoCliente;
            if (tipoCliente == null)
            {
                return NotFound("Cliente no encontrado para esta orden.");
            }

            // Actualizar el total de la orden
            await UpdateOrdenTotalAsync(id.Value);

            // Recargar la orden para obtener el total actualizado
            orden = await _context.Orden
                .Include(o => o.IdReservaNavigation)
                    .ThenInclude(r => r.CiClienteNavigation)
                .Include(o => o.IdUsuarioNavigation)
                .Include(o => o.OrdenDetalles)
                    .ThenInclude(od => od.CodigoProdNavigation)
                .Include(o => o.Pagos)
                .FirstOrDefaultAsync(o => o.Id == id);

            var clima = await _weatherService.GetTemperatureAsync();
            var discountPercentage = CalculateDiscountPercentage(clima.Temperatura, clima.Descripcion, tipoCliente);
            var totalConDescuento = orden.Total - (orden.Total * (double)discountPercentage / 100);

            ViewBag.TotalConDescuento = totalConDescuento;
            ViewBag.discountPercentage = discountPercentage;
            return View(orden);
        }

        // GET: Orden/Create
        public async Task<IActionResult> Create(int idReserva)
        {
            var reserva = await _context.Reservas
                .Include(r => r.CiClienteNavigation) 
                .FirstOrDefaultAsync(r => r.Id == idReserva);

            if (reserva == null)
            {
                return NotFound();
            }

            // Obtener el clima
            var clima = await _weatherService.GetTemperatureAsync();

            // Obtener el cliente
            var cliente = reserva.CiClienteNavigation;
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado para esta reserva.");
            }

            // Calcular el descuento
            var discountPercentage = CalculateDiscountPercentage(clima.Temperatura, clima.Descripcion, cliente.TipoCliente);

            var orden = new Orden
            {
                IdReserva = idReserva,
                Estado = "Activa",
                OrdenDetalles = new List<OrdenDetalle>()
            };

            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id");
            ViewBag.DiscountPercentage = discountPercentage;
            return View(orden);
        }


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
            var orden = await _context.Orden
                .Include(o => o.OrdenDetalles)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            _context.OrdenDetalles.RemoveRange(orden.OrdenDetalles);

            _context.Orden.Remove(orden);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         
        }

        //VERIFICAR QUE EXISTE LA ORDEN
        private bool OrdenExists(int id)
        {
            return _context.Orden.Any(e => e.Id == id);
        }


        //CHEQUEAR SI EXISTE UNA ORDEN ASOCIADA A UNA RESERVA O NO. EN ESE CASO CREARLA
        public async Task<IActionResult> CheckOrder(int idReserva)
        {
            var ordenExistente = await _context.Orden.FirstOrDefaultAsync(o => o.IdReserva == idReserva);

            if (ordenExistente == null)
            {
                return RedirectToAction("Create", new { IdReserva = idReserva });
            }

            return RedirectToAction("Details", new { id = ordenExistente.Id });
        }

        //ACTUALIZAR ESTADO DE LA ORDEN
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

        //CALCULO DE DESCUENTOS DE UN CLIENTE
        private double CalculateDiscountPercentage(double temperature, string description, string tipoCliente)
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

            if (tipoCliente != null)
            {
                switch (tipoCliente)
                {
                    case "VIP":
                        discount += 20;
                        break;
                    case "Frecuente":
                        discount += 10;
                        break;
                    case "Nuevo":
                        discount += 5;
                        break;
                    default:
                        discount += 0;
                        break;
                }
            }
                return discount;
        }

        //ACTUALIZAR TOTALES SEGUN DESCUENTOS A MEDIDA QUE SE AGREGAN PRODUCTOS
        private async Task UpdateOrdenTotalAsync(int idOrden)
        {
            var orden = await _context.Orden
        .Include(o => o.OrdenDetalles)
            .ThenInclude(od => od.CodigoProdNavigation)
        .Include(o => o.IdReservaNavigation)
            .ThenInclude(r => r.CiClienteNavigation)
        .FirstOrDefaultAsync(o => o.Id == idOrden);

            if (orden != null)
            {
                // Calcular el total de la orden sin descuento
                orden.Total = orden.OrdenDetalles
                    .Where(od => od.CodigoProdNavigation != null)
                    .Sum(od => od.Cantidad * od.CodigoProdNavigation.Precio);

                // Obtener el clima
                var clima = await _weatherService.GetTemperatureAsync();

                // Obtener el tipo de cliente
                var tipoCliente = orden.IdReservaNavigation?.CiClienteNavigation?.TipoCliente;

                if (tipoCliente != null)
                {
                    // Calcular el descuento total
                    double descuentoTotal = CalculateDiscountPercentage(clima.Temperatura, clima.Descripcion, tipoCliente);

                    // Aplicar el descuento
                    orden.Total -= orden.Total * descuentoTotal / 100;

                    // Guardar los cambios
                    _context.Update(orden);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("Cliente no encontrado para la orden especificada.");
                }
            }
        }


        //LISTA DE ORDENES
        public async Task<IActionResult> ListaOrdenes()
        {
            var restauranteContext = _context.Orden.Include(o => o.IdReservaNavigation).Include(o => o.IdUsuarioNavigation);
            return View(await restauranteContext.ToListAsync());
        }
    }             
}

