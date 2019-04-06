using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using WebBackCurrencyConverter.API.Repositories;

namespace WebBackCurrencyConverter.Test.Repositories
{
    [TestClass]
    public class CurrencyRatesRepositoryTest
    {
        private readonly Mock<HttpClient> _httpClientTestDouble;

        public CurrencyRatesRepositoryTest()
        {
            var xml = File.ReadAllText("assets/currencyratesxml.xml");

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(xml)
                })
                .Verifiable();

            _httpClientTestDouble = new Mock<HttpClient>(handlerMock.Object);
        }

        [TestMethod]
        public async Task GetCurrencyRates_WhenNoOfRatesIs33_Expect33Rates()
        {
            // Arrange
            var sut = new CurrencyRatesRepository(_httpClientTestDouble.Object);
            const int expected = 33;

            // Act
            var actual = await sut.GetCurrencyRates();

            // Assert
            Assert.AreEqual(expected, actual.Count());
        }

        [TestMethod]
        public async Task GetCurrencyRate_WhenCodeIsEUR_ExpectEuroRate()
        {
            // Arrange
            var sut = new CurrencyRatesRepository(_httpClientTestDouble.Object);

            const string code = "eur";
            const float expected = 746.39f;

            // Act
            var actual = await sut.GetCurrencyRateByCode(code);

            // Assert
            Assert.AreEqual(expected, actual.Rate);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Currency code xyz not found")]
        public async Task GetCurrencyRate_WhenCodeIsNotFound_ExpectException()
        {
            // Arrange
            var sut = new CurrencyRatesRepository(_httpClientTestDouble.Object);

            const string code = "xyz";

            // Act
            await sut.GetCurrencyRateByCode(code);
        }
    }
}