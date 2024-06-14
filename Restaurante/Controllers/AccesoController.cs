using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Restaurante.Models;
using Microsoft.AspNetCore.Http;
using System.Data;
using Newtonsoft.Json;


namespace Restaurante.Controllers
{
     public class AccesoController : Controller
    {
        private string CadenaSQL = "Data Source=LAPTOP-CCI9Q8VH ;Initial Catalog= restaurante;Integrated Security=True; TrustServerCertificate=True";

        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            using (SqlConnection cn = new SqlConnection(CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("ValidarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                

                cn.Open();

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    usuario.Id = Convert.ToInt32(result);
                }
                else
                {
                    usuario.Id = 0;
                }


            }

            if (usuario.Id != 0)
            {
                return RedirectToAction("Index", "Producto");

            } else
            {
                TempData["Mensaje"] = "Usuario no encontrado";
                return View();
            }
            
        }
    }
}
