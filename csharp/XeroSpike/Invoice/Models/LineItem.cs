using System;
using Newtonsoft.Json;

namespace Invoice.Models
{
    public class LineItem
    {
        public Guid Id { get; set; }
        
        public String Description { get; set; }
        
        public Decimal UnitAmount { get; set; }
        
        public Decimal TaxAmount { get; set; }
        
        public Decimal LineAmount { get; set; }
        
        public Double Quantity { get; set; }
        
        [JsonIgnore] 
        public Invoice Invoice { get; set; }
    }
}