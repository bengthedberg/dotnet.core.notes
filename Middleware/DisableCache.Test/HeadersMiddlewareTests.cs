using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DisableCache.Test
{
    public partial class HeadersMiddlewareTests
    {
        [Test]
        public async Task TestCacheDisabledAsync()
        {
            // Arrange
            using var client = CreateClient();

            // Act 
            var response = await client.GetAsync("/");

            // Assert
            Assert.AreEqual("no-cache", response.Headers.GetValues("Cache-Control").FirstOrDefault());

        }


        private static HttpClient CreateClient()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment(Environments.Development)
                .Configure(app => app
                    .UseDisabledCacheHeaders());
            ;

            var server = new TestServer(builder);

            return server.CreateClient();
        }
    }
}