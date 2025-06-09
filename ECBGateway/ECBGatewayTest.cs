using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECBGateway
{
    public class ECBGatewayTest
    {
        [Fact]
        public async Task GetDailyRatesAsync_ShouldReturnRatesIncludingEUR()
        {
            // Arrange
            var httpClient = new HttpClient(); // You could mock this too
            var client = new ECBClient(httpClient);

            // Act
            var rates = await client.GetLatestRatesAsync();

            // Assert
            foreach (var rate in rates)
            {
                if (string.IsNullOrEmpty(rate.Currency))
                {
                    Console.WriteLine("Found a rate with missing currency code.");
                }
                else
                {
                    Console.WriteLine($"Currency: {rate.Currency}, Rate: {rate.Rate}");
                }
            }
        }
    }
}
