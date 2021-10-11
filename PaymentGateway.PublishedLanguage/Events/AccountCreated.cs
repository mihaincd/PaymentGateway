using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class AccountCreated
    {
        public double Sold { get; set; }
        public string Cnp { get; set; }
        public string Curency { get; set; }

        public AccountCreated(double sold, string cnp, string curency)
        {
            Sold = sold;
            Cnp = cnp;
            Curency = curency;
        }
    }
}
