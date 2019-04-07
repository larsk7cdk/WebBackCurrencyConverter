using System.Threading.Tasks;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebBackCurrencyConverter.API.Models;
using WebBackCurrencyConverter.API.Services;

namespace WebBackCurrencyConverter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;
        private readonly ILogger<ExchangeController> _logger;

        public ExchangeController(IExchangeService exchangeService, ILogger<ExchangeController> logger)
        {
            _exchangeService = exchangeService;
            _logger = logger;
        }

        /// <summary>
        ///     Omregner beløb fra en valuta til anden valuta
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /Exchange
        ///     {
        ///     "amount": 100,
        ///     "fromCurrencyCode": "dkk"
        ///     "fromCurrencyCode": "eur"
        ///     }
        /// </remarks>
        /// <param name="amount"></param>
        /// <param name="fromCurrencyCode"></param>
        /// <param name="toCurrencyCode"></param>
        /// <returns>Omregnet valuta</returns>
        /// <response code="200">Returner omregnet beløb, valuta samt kurs</response>
        /// <response code="400">Hvis beløb er under 0</response>
        [HttpPost("Exchange")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Exchange([FromBody] ExchangeRequest request)
        {
            _logger.LogInformation(
                $"Omregn beløbet {request.Amount} med fra valuta \"{request.FromCurrencyCode}\" til valuta \"{request.ToCurrencyCode}\"");

            if (!ModelState.IsValid) return BadRequest();

            var result =
                await _exchangeService.Exchange(request.Amount, request.FromCurrencyCode, request.ToCurrencyCode);

            return this.HAL(result, new[]
            {
                new Link("self", "/Exchange?amount=0&currencycode=eur", "ExchangeFromDkk",
                    "GET"),
                new Link("ExchangeToDkk", "/ExchangeToDkk?amount=0&currencycode=eur", "ExchangeFromDkk", "GET")
            });
        }
    }
}