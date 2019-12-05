using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Invoice;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        [Fact]
        public async Task ShouldGetListOfInvoices()
        {
          
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(builder);
            var dataContext = (DataContext)server.Services.GetService(typeof(DataContext));
            
            var fixture = new Fixture();
            
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var invoices = fixture.CreateMany<Invoice.Models.Invoice>(2).ToList();
            
            invoices.ForEach(x => dataContext.Invoices.Add(x));
            dataContext.SaveChanges();
            
            // setup
            var client = server.CreateClient();
            
            // when
            var response = await client.GetAsync("api/invoice");
            
            var listOfInvoice = JsonConvert.DeserializeObject<List<Invoice.Models.Invoice>>(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, listOfInvoice.Count);
        }

        [Fact]
        public async Task ShouldCreateAnInvoice()
        {
            // setup
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(builder);
            var client = server.CreateClient();

            // when
            var payload = new
            {
                InvoiceNumber = "123",
                AmountPaid = "123.43",
                Total = "123",
                Hello = new List<Object>
                {
                    new
                    {
                        Description = "chicken nuggets",
                        UnitAmount = "24.00",
                        TaxAmount = "234.23",
                        LineAmount = "34.34",
                        Quantity = "34.00"
                    }
                }
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response =
                await client.PostAsync("api/invoice",
                    new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

            // then

            var model = JsonConvert.DeserializeObject<Invoice.Models.Invoice>(response.Content.ReadAsStringAsync().Result);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.AmountPaid, model.AmountPaid.ToString(CultureInfo.InvariantCulture));
        }
    }
}