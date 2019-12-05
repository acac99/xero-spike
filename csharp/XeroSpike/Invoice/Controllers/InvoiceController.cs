namespace Invoice.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {
        private readonly DataContext dataContext;

        public InvoiceController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Invoices()
        {
            var invoices = dataContext.Invoices.Include(x => x.LineItems).ToList();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public IActionResult Invoice(Guid id)
        {
            var invoice = dataContext.Invoices.Where(x => x.Id == id).Include(x => x.LineItems)
                .FirstOrDefault();
            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        [HttpPost]
        public IActionResult SaveInvoice([FromBody] Models.Invoice invoice)
        {
            dataContext.Invoices.Add(invoice);
            invoice.LineItems.ForEach(x => dataContext.LineItems.Add(x));
            dataContext.SaveChanges();
            return Ok(invoice);
        }
    }
}