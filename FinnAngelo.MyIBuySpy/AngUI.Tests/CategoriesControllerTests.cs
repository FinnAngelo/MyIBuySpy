using FinnAngelo.MyIBuySpy.AngUI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace AngUI.Tests
{
    [TestClass]
    public class CategoriesControllerTests
    {
        [TestMethod]
        public async Task When_CategoriesController()
        {
            using WebApplicationFactory<Startup> factory = new WebApplicationFactory<Startup>();
            // Arrange
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("Categories");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.AreEqual("text/plain", response.Content.Headers.ContentType.ToString());
            Assert.AreEqual("Healthy", await response.Content.ReadAsStringAsync());
        }
    }
}
