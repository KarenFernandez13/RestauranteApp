using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    public class ListasController : Controller
    {
        public IActionResult Listas()
        {
            return View();
        }
    }
}
