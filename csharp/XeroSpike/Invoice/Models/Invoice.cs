using System;
using System.Collections.Generic;

namespace Invoice.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        
        public string InvoiceNumber { get; set; }
        
        public Decimal AmountPaid { get; set; }
        
        public Decimal Total { get; set; }

        public List<LineItem> LineItems { get; set; }
    }
}