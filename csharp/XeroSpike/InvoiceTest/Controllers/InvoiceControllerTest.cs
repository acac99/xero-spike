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
using Newtonsoft.Json;
using Xunit;

namespace InvoiceTest.Controllers
{
    public class InvoiceControllerTest : IDisposable
    {
        private HttpClient _client;
        private DataContext _dataContext;
        public InvoiceControllerTest()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(builder);
            _client = server.CreateClient();
;           _dataContext = (DataContext)server.Services.GetService(typeof(DataContext));
        }
        [Fact]
        public async Task ShouldExpect200OnGet()
        {

            // when
            var response = await _client.GetAsync("api/invoice");

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldGetListOfInvoices()
        {
            var fixture = new Fixture();
            
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var invoices = fixture.CreateMany<Invoice.Models.Invoice>(2).ToList();
            
            invoices.ForEach(x => _dataContext.Invoices.Add(x));
            _dataContext.SaveChanges();
            
            // when
            var response = await _client.GetAsync("api/invoice");
            
            var listOfInvoice = JsonConvert.DeserializeObject<List<Invoice.Models.Invoice>>(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, listOfInvoice.Count);
        }
        
        [Fact]
        public async Task ShouldAnInvoiceByItsId()
        {

            var fixture = new Fixture();
            
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            
            var invoice = fixture.Create<Invoice.Models.Invoice>();

            _dataContext.Invoices.Add(invoice);
            _dataContext.SaveChanges();
            
            // when
            var response = await _client.GetAsync($"api/invoice/{invoice.Id}");
            
            var invoiceResponse = JsonConvert.DeserializeObject<Invoice.Models.Invoice>(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(invoice.Id, invoiceResponse.Id);
        }

        [Fact]
        public async Task ShouldCreateAnInvoice()
        {

            // when
            var payload = new
            {
                InvoiceNumber = "123",
                AmountPaid = "123.43",
                Total = "123",
                LineItems = new List<Object>
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

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response =
                await _client.PostAsync("api/invoice",
                    new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

            // then

            var model = JsonConvert.DeserializeObject<Invoice.Models.Invoice>(response.Content.ReadAsStringAsync().Result);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(payload.AmountPaid, model.AmountPaid.ToString(CultureInfo.InvariantCulture));
        }

        public void Dispose()
        {
            _dataContext.Database.EnsureDeleted();
        }
    }
}