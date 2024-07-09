namespace Restaurante.Models
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "295f3f23f279c767fb7f4a51458197e1"; 

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Clima> GetTemperatureAsync()
        {
            string url = $"http://api.openweathermap.org/data/2.5/weather?lat=-34.91&lon=-54.96&units=metric&lang=es&appid={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode} and message: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            double temperature = json["main"]["temp"].Value<double>();

            var clima = new Clima
            {
                Fecha = DateOnly.FromDateTime(DateTime.UtcNow),
                Temperatura = temperature,
                Descripcion = json["weather"][0]["description"].Value<string>()
            };

            return clima;
        }

    }
}
