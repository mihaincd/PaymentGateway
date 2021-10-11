using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class ProductCreated
    {
        public double Value { get; set; }
        public string Name { get; set; }
    }
}
