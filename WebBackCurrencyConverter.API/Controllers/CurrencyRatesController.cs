using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebBackCurrencyConverter.API.Entities;
using WebBackCurrencyConverter.API.Repositories;

namespace WebBackCurrencyConverter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly ILogger<CurrencyRatesController> _logger;

        public CurrencyRatesController(ICurrencyRatesRepository currencyRatesRepository, ILogger<CurrencyRatesController> logger)
        {
            _currencyRatesRepository = currencyRatesRepository;
            _logger = logger;
        }

        /// <summary>
        /// Henter alle dagens valuta kurser fra Nationalbanken
        /// </summary>
        /// <returns>
        /// Liste af dagens valuta kurser
        /// </returns>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<CurrencyRate>>> Get()
        {
            _logger.LogInformation("´Bruger har forespurgt på valuta kurser");
            return await _currencyRatesRepository.GetCurrencyRates();
        }

        /// <summary>
        /// Henter dagens valuta kurs fra Nationalbanken
        /// </summary>
        /// <param name="code">
        /// Angiv valuta for kurs
        /// </param>
        /// <returns>
        /// Dagens valuta kurs
        /// </returns>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("{code}")]
        public async Task<ActionResult<CurrencyRate>> Get(string code) =>
            await _currencyRatesRepository.GetCurrencyRateByCode(code);
    }
}