using Microsoft.AspNetCore.Mvc;
using currency.Shared;
using currency.Shared.Infrastructure;

namespace currency.Features.Currency.GetAll.Endpoints
{
    [ApiController]
    public class GetAllCurrenciesEndpoint : ControllerBase
    {
        private readonly CurrencyStorage _currencyStorage;
        private readonly ILogger<GetAllCurrenciesEndpoint> _logger;

        public GetAllCurrenciesEndpoint(CurrencyStorage currencyStorage, ILogger<GetAllCurrenciesEndpoint> logger)
        {
            _currencyStorage = currencyStorage;
            _logger = logger;
        }

        [HttpGet("api/currency")]
        public async Task<ActionResult<IEnumerable<Shared.Currency>>> Handle()
        {
            try
            {
                var currencies = await _currencyStorage.GetAllCurrencies();
                return Ok(currencies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving currencies");
                return StatusCode(500, "An error occurred while retrieving currencies");
            }
        }
    }
}