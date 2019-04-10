using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebBackCurrencyConverter.API.Entities;

namespace WebBackCurrencyConverter.API.Repositories
{
    public interface ICurrencyRatesRepository
    {
        Task<List<CurrencyRate>> GetCurrencyRates();
        Task<CurrencyRate> GetCurrencyRateByCode(string code);
    }

    public class CurrencyRatesRepository : ICurrencyRatesRepository
    {
        private static readonly List<CurrencyRate> CurrencyRates = new List<CurrencyRate>();
        private readonly HttpClient _httpClient;

        public CurrencyRatesRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CurrencyRate>> GetCurrencyRates()
        {
            if (CurrencyRates.Count == 0)
                await GetData();

            return CurrencyRates;
        }

        public async Task<CurrencyRate> GetCurrencyRateByCode(string code)
        {
            if (code.Equals("dkk", StringComparison.CurrentCultureIgnoreCase))
            {
                return new CurrencyRate
                {
                    Code = "DKK",
                    Description = "Danske kroner",
                    Rate = 100
                };
            }

            if (CurrencyRates.Count == 0)
                await GetCurrencyRates();

            var cr = CurrencyRates.FirstOrDefault(x =>
                string.Equals(x.Code, code, StringComparison.CurrentCultureIgnoreCase));


            if (cr == null)
                throw new Exception($"Currency code {code} not found");


            return cr;
        }

        private async Task GetData()
        {
            var xml = await GetStringAsync();

            await Task.Run(() =>
            {
                var sr = new StringReader(xml);
                var exchangerates = (Exchangerates)new XmlSerializer(typeof(Exchangerates)).Deserialize(sr);

                foreach (var currency in exchangerates.Dailyrates.Currency)
                    CurrencyRates.Add(new CurrencyRate
                    {
                        Code = currency.Code,
                        Description = currency.Desc,
                        Rate = float.Parse(currency.Rate)
                    });
            });
        }

        private async Task<string> GetStringAsync()
        {
            const string url = "http://www.nationalbanken.dk/_vti_bin/DN/DataService.svc/CurrencyRatesXML?lang=da";

            var response = await _httpClient.GetAsync(url);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty;
        }
    }
}