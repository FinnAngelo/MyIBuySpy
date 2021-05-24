using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AngUI.Tests
{
    internal static class FactoryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStartup">Startup class for test server</typeparam>
        /// <param name="factory"></param>
        /// <returns>An authenticated HttpClient</returns>
        /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0#mock-authentication"/>
        public static HttpClient GetAuthenticatedClient<TStartup>(this WebApplicationFactory<TStartup> factory) where TStartup: class
        {
            HttpClient client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            return client;
        }
    }
}
