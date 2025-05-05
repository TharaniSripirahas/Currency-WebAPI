using Microsoft.AspNetCore.Mvc;
using currency.Shared;
using currency.Shared.Infrastructure;

namespace currency.Features.Currency.Add.Endpoints
{
    [ApiController]
    public class AddCurrencyEndpoint : ControllerBase
    {
        private readonly CurrencyStorage _currencyStorage;
        private readonly ILogger<AddCurrencyEndpoint> _logger;

        public AddCurrencyEndpoint(CurrencyStorage currencyStorage, ILogger<AddCurrencyEndpoint> logger)
        {
            _currencyStorage = currencyStorage;
            _logger = logger;
        }

        [HttpPost("api/currency")]
        public async Task<ActionResult<Shared.Currency>> Handle([FromBody] Shared.Currency currency)
        {
            try
            {
                var result = await _currencyStorage.AddCurrency(currency);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding currency");
                return StatusCode(500, "An error occurred while adding the currency");
            }
        }
    }
}