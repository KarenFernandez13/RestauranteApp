using Microsoft.AspNetCore.Mvc;
using Restaurante.Models;

namespace Restaurante.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index()
        {
            var clima = await _weatherService.GetTemperatureAsync();
            ViewBag.Clima = clima;
            return View();
        }
    }
}
