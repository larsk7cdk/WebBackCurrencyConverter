using System.Threading.Tasks;
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
        public async Task<double> ExchangeFromDkk([FromQuery]double amount, [FromQuery]string currencyCode)
        {
            return await _exchangeService.ExchangeFromDkk(amount,currencyCode);
        }

        [HttpGet("ExchangeToDkk")]
        public async Task<double> ExchangeToDkk([FromQuery]double amount, [FromQuery]string currencyCode)
        {
            return await _exchangeService.ExchangeToDkk(amount,currencyCode);
        }
    }
}