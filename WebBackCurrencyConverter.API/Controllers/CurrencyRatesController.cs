using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBackCurrencyConverter.API.Models;
using WebBackCurrencyConverter.API.Repositories;

namespace WebBackCurrencyConverter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly ICurrencyRatesRepository _currencyRatesRepository;

        public CurrencyRatesController(ICurrencyRatesRepository currencyRatesRepository)
        {
            _currencyRatesRepository = currencyRatesRepository;
        }

        /// <summary>
        /// Henter alle dagens valuta kurser fra Nationalbanken
        /// </summary>
        /// <returns>
        /// Liste af dagens valuta kurser
        /// </returns>
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<CurrencyRate>>> Get() =>
            await _currencyRatesRepository.GetCurrencyRates();


        [HttpGet("{code}")]
        public async Task<ActionResult<CurrencyRate>> Get(string code) =>
            await _currencyRatesRepository.GetCurrencyRateByCode(code);
    }
}