using Microsoft.AspNetCore.Mvc;
using Restaurante.Models;
using System.Diagnostics;
<<<<<<< HEAD
=======


>>>>>>> cc0ce3aea1527563f64bae33cea5b5c05b7261a2
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Restaurante.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
<<<<<<< HEAD
        private readonly WeatherService _weatherService;

        public HomeController(ILogger<HomeController> logger, WeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }


        public async Task<IActionResult> Index()
        {
            Clima clima = null;
            double discountPercentage = 0;
            try
            {
                clima = await _weatherService.GetTemperatureAsync();
                discountPercentage = CalculateDiscountPercentage(clima.Temperatura, clima.Descripcion);
            }
            catch (HttpRequestException ex)
            {
                // Manejar el error aquí, por ejemplo, registrando el mensaje o mostrando un mensaje de error en la vista
                ViewBag.Error = $"No se pudo obtener la temperatura: {ex.Message}";
            }

            ViewBag.Clima = clima;
            ViewBag.DiscountPercentage = discountPercentage;

            return View();
        }

        private double CalculateDiscountPercentage(double temperature, string description)
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

            return discount;
        }
=======

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

>>>>>>> cc0ce3aea1527563f64bae33cea5b5c05b7261a2

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // para salir de la sesion
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}