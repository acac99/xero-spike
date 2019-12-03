using Microsoft.AspNetCore.Mvc;

namespace Invoice.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {
       [HttpGet]
        public IActionResult Invoices()
        {
            return Ok();
        }
    }
}