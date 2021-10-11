using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
    public class MultiplePurchaseCommand
    {
        public int IdProduct { get; set; }
        public int IdAccount { get; set; }
        public double Quantity { get; set; }
        public double Value { get; set; }
        public string Curency { get; set; }
    }
}
