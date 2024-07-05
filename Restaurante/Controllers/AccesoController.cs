using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Restaurante.Models;
using Microsoft.AspNetCore.Http;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// para poder guardar la sesion
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Restaurante.Controllers
{
    public class AccesoController : Controller
    {
        private readonly RestauranteContext _context;

        public AccesoController(RestauranteContext context)
        {
            _context = context;
        }

        public ActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Producto");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
              var usuario_Encontrado = await _context.Usuarios
               .Include(u => u.IdRolNavigation)
              .ThenInclude(r => r.Permisos)
                .Where(u => u.Id == usuario.Id && u.Contraseña == usuario.Contraseña)
               .FirstOrDefaultAsync();
        

            if (usuario_Encontrado == null)
            {
                TempData["Mensaje"] = "Usuario no encontrado";
                return View();
            }

            var permisos = usuario_Encontrado.IdRolNavigation.Permisos.ToList();

            List<Claim> claims = new List<Claim>
               {
                   new Claim(ClaimTypes.NameIdentifier, usuario_Encontrado.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario_Encontrado.Nombre),
                    new Claim(ClaimTypes.Role, usuario_Encontrado.IdRol.ToString())
                };


            foreach (Permiso p in permisos)
            {
                claims.Add(new Claim(ClaimTypes.Role, p.Descripcion));

                if (!string.IsNullOrEmpty(p.Numero.ToString()))
                {
                    claims.Add(new Claim("Permission", p.Descripcion));
                }
            }


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true // Mantener la sesión iniciada entre reinicios del navegador
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            if (usuario_Encontrado.IdRol == 4)
            {
                return RedirectToAction("Create", "Reseña");
            }
            else
            {
                return RedirectToAction("Index", "Producto");
            }
        }
    }
}
