using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public string Curency { get; set; }
        public double Limit { get; set; }
    }
}
