namespace InvoiceTest.Controllers
{
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

    public sealed class InvoiceControllerTest : IDisposable
    {
        private readonly HttpClient client;
        private readonly DataContext dataContext;

        public InvoiceControllerTest()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(builder);
            client = server.CreateClient();
            dataContext = (DataContext)server.Services.GetService(typeof(DataContext));
        }

        [Fact]
        public async Task ShouldExpect200OnGet()
        {
            var uri = new Uri("https://localhost:5001/api/invoice");

            // when
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldGetListOfInvoices()
        {
            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var invoices = fixture.CreateMany<Invoice.Models.Invoice>(2).ToList();

            invoices.ForEach(x => dataContext.Invoices.Add(x));
            dataContext.SaveChanges();

            // when
            var uri = new Uri("https://localhost:5001/api/invoice");
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            var listOfInvoice =
                JsonConvert.DeserializeObject<List<Invoice.Models.Invoice>>(response.Content
                    .ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, listOfInvoice.Count);
        }

        [Fact]
        public async Task ShouldAnInvoiceByItsId()
        {
            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var invoice = fixture.Create<Invoice.Models.Invoice>();

            dataContext.Invoices.Add(invoice);
            dataContext.SaveChanges();

            // when
            var uri = new Uri($"https://localhost:5001/api/invoice/{invoice.Id}");
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            var invoiceResponse =
                JsonConvert.DeserializeObject<Invoice.Models.Invoice>(response.Content
                    .ReadAsStringAsync().Result);
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
                LineItems = new List<object>
                {
                    new
                    {
                        Description = "chicken nuggets",
                        UnitAmount = "24.00",
                        TaxAmount = "234.23",
                        LineAmount = "34.34",
                        Quantity = "34.00",
                    },
                },
            };

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var uri = new Uri("https://localhost:5001/api/invoice");
            var response =
                await client.PostAsync(
                    uri,
                    new StringContent(
                        JsonConvert.SerializeObject(payload),
                        Encoding.UTF8,
                        "application/json"))
                    .ConfigureAwait(false);

            // then
            var model =
                JsonConvert.DeserializeObject<Invoice.Models.Invoice>(response.Content
                    .ReadAsStringAsync().Result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(
                payload.AmountPaid,
                model.AmountPaid.ToString(CultureInfo.InvariantCulture));
        }

        public void Dispose()
        {
            dataContext.Database.EnsureDeleted();
        }
    }
}