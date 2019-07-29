using OrderAndInvoicingSystem3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAndInvoicingSystem3.Models
{

    [NotMapped]
    public partial class InvoiceOrder
    {
        public string Product_name { get; set; }
        public decimal Rate { get; set; }
        public decimal GST_rate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal GST { get; set; }
        public decimal Total_price { get; set; }
                
    }
}