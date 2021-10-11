using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class ProductPurchased
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public string Curency { get; set; }
        public int Limit { get; set; }

    }
}
