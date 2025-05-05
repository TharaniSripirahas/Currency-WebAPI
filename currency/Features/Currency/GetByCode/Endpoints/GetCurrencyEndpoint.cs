using Microsoft.AspNetCore.Mvc;
using currency.Shared;
using currency.Shared.Infrastructure;

namespace currency.Features.Currency.GetByCode.Endpoints
{
    [ApiController]
    public class GetCurrencyEndpoint : ControllerBase
    {
        private readonly CurrencyStorage _currencyStorage;
        private readonly ILogger<GetCurrencyEndpoint> _logger;

        public GetCurrencyEndpoint(CurrencyStorage currencyStorage, ILogger<GetCurrencyEndpoint> logger)
        {
            _currencyStorage = currencyStorage;
            _logger = logger;
        }

        [HttpGet("api/currency/{currencyCode}")]
        public async Task<ActionResult<Shared.Currency>> Handle(string currencyCode)
        {
            try
            {
                var currency = await _currencyStorage.GetCurrencyByCode(currencyCode);
                if (currency == null)
                {
                    return NotFound($"Currency with code {currencyCode} not found");
                }
                return Ok(currency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving currency");
                return StatusCode(500, "An error occurred while retrieving the currency");
            }
        }
    }
}