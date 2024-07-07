using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Restaurante.Controllers
{
    public class ListasController : Controller
    {
        [Authorize(Policy = "CanVerListados")]
        public IActionResult Listas()
        {
            return View();
        }
    }
}
