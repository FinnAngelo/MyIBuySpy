using FinnAngelo.MyIBuySpy.AngUI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace AngUI.Tests.WeatherForecast
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        [TestMethod]
        public async Task Given_AuthenticatedClient_When_Get()
        {
            using WebApplicationFactory<Startup> factory = new WebApplicationFactory<Startup>();
            // Arrange
            var client = factory.GetAuthenticatedClient();

            // Act
            var response = await client.GetAsync("WeatherForecast");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var result  = await JsonSerializer.DeserializeAsync<dynamic[]>(
                await response.Content.ReadAsStreamAsync()
                );
            Assert.AreEqual(5, result.Length);
        }
    }
}
