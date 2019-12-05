namespace Invoice.Models
{
    using System;
    using System.Collections.Generic;

    public class Invoice
    {
        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Total { get; set; }

        public List<LineItem> LineItems { get; set; }
    }
}