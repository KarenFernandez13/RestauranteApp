﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Models;

namespace Restaurante.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly RestauranteContext _context;

        public UsuarioController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            var restauranteContext = _context.Usuarios.Include(u => u.IdRolNavigation).Include(u => u.IdSucursalNavigation);
            return View(await restauranteContext.ToListAsync());
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .Include(u => u.IdSucursalNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id");
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id");
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Contraseña,Email,IdSucursal,IdRol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id", usuario.IdRol);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id", usuario.IdSucursal);
            return View(usuario);
        }
        public IActionResult CreateCliente()
        {
            //ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id");
            //ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id");
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCliente([Bind("Id,Nombre,Contraseña,Email")] Usuario usuario)
        {
            var listaClientes = await _context.Usuarios.FindAsync(usuario.Id);
            if (listaClientes == null)
            {
                if (ModelState.IsValid)
                {
                    usuario.IdRol = 4;
                    usuario.IdSucursal = 1;
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Acceso");
                }
            }
            else
            {
                TempData["Mensaje"] = "Cliente registrado";
                return View();
            }

            //ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id", usuario.IdRol);
            //ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id", usuario.IdSucursal);
            return View(usuario);
        }


        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id", usuario.IdRol);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id", usuario.IdSucursal);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Contraseña,Email,IdSucursal,IdRol")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Id", usuario.IdRol);
            ViewData["IdSucursal"] = new SelectList(_context.Sucursals, "Id", "Id", usuario.IdSucursal);
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .Include(u => u.IdSucursalNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
