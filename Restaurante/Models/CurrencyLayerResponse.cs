namespace Restaurante.Models
{
    public class CurrencyLayerResponse
    {
        public bool Success { get; set; }
        public Dictionary<string, decimal> Quotes { get; set; }

    }
}
