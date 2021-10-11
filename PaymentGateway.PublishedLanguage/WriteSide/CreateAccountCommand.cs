using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
    public class CreateAccountCommand
    {
        public int? PersonId { get; set; }
        public double Sold { get; set; }
        public double Limit { get; set; }
        public string IbanCode { get; set; }
        public string Cnp { get; set; }
        public string Curency { get; set; }
        public string ClientType { get; set; }
        public string Status { get; set; }


    }
}
