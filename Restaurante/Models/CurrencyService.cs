using Newtonsoft.Json.Linq;

namespace Restaurante.Models
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "c9fdb6d5e291d3dda0d24ccdd9f97fcd\r\n"; 

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            string url = $"http://api.currencylayer.com/live?access_key={_apiKey}&currencies={toCurrency}&source={fromCurrency}&format=1";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode} and message: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            string key = $"{fromCurrency}{toCurrency}";
            decimal exchangeRate = json["quotes"][key].Value<decimal>();

            return exchangeRate;
        }
    }
}
