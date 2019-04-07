using System;
using System.Threading.Tasks;
using WebBackCurrencyConverter.API.Models;
using WebBackCurrencyConverter.API.Repositories;

namespace WebBackCurrencyConverter.API.Services
{
    public interface IExchangeService
    {
        Task<ExchangeResult> Exchange(double amount, string fromCurrencyCode, string toCurrencyCode);
    }

    public class ExchangeService : IExchangeService
    {
        private readonly ICurrencyRatesRepository _currencyRatesRepository;

        public ExchangeService(ICurrencyRatesRepository currencyRatesRepository)
        {
            _currencyRatesRepository = currencyRatesRepository;
        }

        public async Task<ExchangeResult> Exchange(double amount, string fromCurrencyCode, string toCurrencyCode)
        {
            if (fromCurrencyCode.Equals("dkk"))
                return await ExchangeFromDkk(amount, toCurrencyCode);
            return await ExchangeToDkk(amount, fromCurrencyCode);
        }

        private async Task<ExchangeResult> ExchangeFromDkk(double amount, string currencyCode)
        {
            var currencyRate = await _currencyRatesRepository.GetCurrencyRateByCode(currencyCode);
            var result = Math.Round(100 / currencyRate.Rate * amount, 2);

            return new ExchangeResult
            {
                Amount = result,
                CurrencyCode = currencyCode,
                Rate = currencyRate.Rate
            };
        }

        private async Task<ExchangeResult> ExchangeToDkk(double amount, string currencyCode)
        {
            var currencyRate = await _currencyRatesRepository.GetCurrencyRateByCode(currencyCode);
            var result = Math.Round(currencyRate.Rate / 100 * amount, 2);

            return new ExchangeResult
            {
                Amount = result,
                CurrencyCode = currencyCode,
                Rate = currencyRate.Rate
            };
        }
    }
}