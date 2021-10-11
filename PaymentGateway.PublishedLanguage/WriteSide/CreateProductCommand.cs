using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
    public class CreateProductCommand
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public string Curency { get; set; }
        public double Limit { get; set; }

        }
}
