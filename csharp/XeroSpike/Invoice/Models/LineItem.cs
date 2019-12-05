namespace Invoice.Models
{
    using System;
    using Newtonsoft.Json;

    public class LineItem
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public decimal UnitAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal LineAmount { get; set; }

        public double Quantity { get; set; }

        [JsonIgnore]
        public Invoice Invoice { get; set; }
    }
}