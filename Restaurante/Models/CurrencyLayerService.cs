using Newtonsoft.Json;

namespace Restaurante.Models
{
    public class CurrencyLayerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey = "c9fdb6d5e291d3dda0d24ccdd9f97fcd"; 

        public CurrencyLayerService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync(string sourceCurrency, List<string> targetCurrencies)
        {
            var currencies = string.Join(",", targetCurrencies);
            var client = _httpClientFactory.CreateClient("currencyLayer");
            var response = await client.GetAsync($"live?access_key=c9fdb6d5e291d3dda0d24ccdd9f97fcd&currencies=USD,EUR&source=UYU&format=1");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CurrencyLayerResponse>(content);

            var rates = new Dictionary<string, decimal>();
            foreach (var currency in targetCurrencies)
            {
                var rateKey = $"{sourceCurrency}{currency}";
                if (data.Quotes.TryGetValue(rateKey, out var rate))
                {
                    rates[currency] = rate;
                }
            }

            return rates;
        }
    }

   
}

