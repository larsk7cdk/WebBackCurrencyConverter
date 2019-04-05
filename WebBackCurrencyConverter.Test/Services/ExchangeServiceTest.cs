using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebBackCurrencyConverter.API.Models;
using WebBackCurrencyConverter.API.Repositories;
using WebBackCurrencyConverter.API.Services;

namespace WebBackCurrencyConverter.Test.Services
{
    [TestClass]
    public class ExchangeServiceTest
    {
        private readonly Mock<ICurrencyRatesRepository> _currencyRatesRepositoryTestDouble = new Mock<ICurrencyRatesRepository>();

        [TestInitialize]
        public void TestInitialize()
        {
            _currencyRatesRepositoryTestDouble.Setup(s => s.GetCurrencyRateByCode("eur")).ReturnsAsync(new CurrencyRate
            {
                Code = "EUR",
                Description = "Euro",
                Rate = 746.39f
            });
        }

        [TestMethod]
        public async Task ExchangeFromDkk_When_Expected()
        {
            // Arrange 
            var sut = new ExchangeService(_currencyRatesRepositoryTestDouble.Object);
            const double expected = 133.98;

            // Act
            var actual = await sut.ExchangeFromDkk(1000, "eur");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ExchangeToDkk_When_Expected()
        {
            // Arrange 
            var sut = new ExchangeService(_currencyRatesRepositoryTestDouble.Object);
            const double expected = 1000.01;

            // Act
            var actual = await sut.ExchangeToDkk(133.98, "eur");

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}