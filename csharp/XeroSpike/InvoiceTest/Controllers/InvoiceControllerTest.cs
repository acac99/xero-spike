using System.Net;
using System.Threading.Tasks;
using Invoice;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace InvoiceTest.Controllers
{
    public class InvoiceControllerTest
    {
        [Fact]
        public async Task ShouldExpect200OnGet()
        {
            // setup
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(builder);
            var client = server.CreateClient();

            // when
            var response = await client.GetAsync("api/invoice");

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}