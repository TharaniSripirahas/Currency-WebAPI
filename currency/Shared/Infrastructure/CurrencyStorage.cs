using System.Text.Json;

namespace currency.Shared.Infrastructure
{
    public class CurrencyStorage
    {
        private readonly string _jsonFilePath;
        private readonly ILogger<CurrencyStorage> _logger;

        public CurrencyStorage(ILogger<CurrencyStorage> logger)
        {
            _logger = logger;
            _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "currencies.json");
            EnsureFileExists();
        }

        private void EnsureFileExists()
        {
            var directory = Path.GetDirectoryName(_jsonFilePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(_jsonFilePath))
            {
                File.WriteAllText(_jsonFilePath, "[]");
            }
        }

        public async Task<List<Currency>> GetAllCurrencies()
        {
            try
            {
                var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);
                return JsonSerializer.Deserialize<List<Currency>>(jsonContent) ?? new List<Currency>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading currencies from JSON file");
                return new List<Currency>();
            }
        }

        public async Task<Currency?> GetCurrencyByCode(string currencyCode)
        {
            var currencies = await GetAllCurrencies();
            return currencies.FirstOrDefault(c => c.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Currency> AddCurrency(Currency currency)
        {
            var currencies = await GetAllCurrencies();
            
            if (currencies.Any(c => c.CurrencyCode.Equals(currency.CurrencyCode, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Currency with code {currency.CurrencyCode} already exists");
            }

            currency.Id = Guid.NewGuid();
            currency.LastUpdated = DateTime.UtcNow;
            currencies.Add(currency);

            await SaveCurrencies(currencies);
            return currency;
        }

        private async Task SaveCurrencies(List<Currency> currencies)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(currencies, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                await File.WriteAllTextAsync(_jsonFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving currencies to JSON file");
                throw;
            }
        }
    }
}