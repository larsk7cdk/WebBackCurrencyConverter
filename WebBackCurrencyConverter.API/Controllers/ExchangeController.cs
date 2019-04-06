using System.Threading.Tasks;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;
using WebBackCurrencyConverter.API.Services;

namespace WebBackCurrencyConverter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpGet("ExchangeFromDkk")]
        public async Task<object> ExchangeFromDkk([FromQuery] double amount, [FromQuery] string currencyCode)
        {
            var result = await _exchangeService.ExchangeFromDkk(amount, currencyCode);

            var response = new
            {
                amount = result,
                code = currencyCode
            };

            //return Ok(response);
            return this.HAL(response, new[]
            {
                new Link("self", "/ExchangeFromDkk?amount=0&currencycode=eur", "ExchangeFromDkk",
                    "GET"),
                new Link("ExchangeToDkk", "/ExchangeToDkk?amount=0&currencycode=eur", "ExchangeFromDkk", "GET")
            });
        }

        [HttpGet("ExchangeToDkk")]
        public async Task<double> ExchangeToDkk([FromQuery] double amount, [FromQuery] string currencyCode)
        {
            return await _exchangeService.ExchangeToDkk(amount, currencyCode);
        }
    }
}