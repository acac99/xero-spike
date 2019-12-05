using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly DataContext _dataContext;

        public InvoiceController(IHttpClientFactory httpClientFactory, DataContext dataContext)
        {
            _httpClientFactory = httpClientFactory;
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Invoices()
        {
            var invoices = _dataContext.Invoices.Include(x => x.LineItems).ToList();
            return Ok(invoices);
        }

        [HttpPost]
        public IActionResult SaveInvoice([FromBody] Models.Invoice invoice)
        {
            _dataContext.Invoices.Add(invoice);
            invoice.LineItems.ForEach(x => _dataContext.LineItems.Add(x));
            _dataContext.SaveChanges();
            return Ok(invoice);
        }
    }
}