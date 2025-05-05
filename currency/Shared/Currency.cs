using System.ComponentModel.DataAnnotations;

namespace currency.Shared
{
    public class Currency
    {
        public Guid Id { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencyCode { get; set; } 

        public decimal ConversionRate { get; set; }

        public decimal CurrentValueInUSD { get; set; }

        public string Country { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Symbol { get; set; }
    }
}