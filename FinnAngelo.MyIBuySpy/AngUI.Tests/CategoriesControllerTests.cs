using FinnAngelo.MyIBuySpy.AngUI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AngUI.Tests
{
    [TestClass]
    public class CategoriesControllerTests
    {
        [TestMethod]
        public async Task When_Get()
        {
            using WebApplicationFactory<Startup> factory = new WebApplicationFactory<Startup>();
            // Arrange
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/Categories");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var result  = await JsonSerializer.DeserializeAsync<dynamic[]>(
                await response.Content.ReadAsStreamAsync()
                );
            Assert.AreEqual(7, result.Length);
        }
    }
}
