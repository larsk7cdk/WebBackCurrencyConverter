using System;
using System.Threading.Tasks;
using WebBackCurrencyConverter.API.Repositories;

namespace WebBackCurrencyConverter.API.Services
{
    public interface IExchangeService
    {
        Task<double> ExchangeFromDkk(double amount, string currencyCode);
        Task<double> ExchangeToDkk(double amount, string currencyCode);
    }

    public class ExchangeService : IExchangeService
    {
        private readonly ICurrencyRatesRepository _currencyRatesRepository;

        public ExchangeService(ICurrencyRatesRepository currencyRatesRepository)
        {
            _currencyRatesRepository = currencyRatesRepository;
        }

        public async Task<double> ExchangeFromDkk(double amount, string currencyCode)
        {
            var currencyRate = await _currencyRatesRepository.GetCurrencyRateByCode(currencyCode);
            return Math.Round((100 / currencyRate.Rate) * amount, 2);
        }

        public async Task<double> ExchangeToDkk(double amount, string currencyCode)
        {
            var currencyRate = await _currencyRatesRepository.GetCurrencyRateByCode(currencyCode);
            return Math.Round((currencyRate.Rate / 100) * amount, 2);

        }
    }
}