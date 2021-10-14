using System;

namespace PaymentGateway.Models
{
    public class Account
    {
        public int IdAccount { get; set; }
        public int IdPerson { get; set; }
        public double Balance { get; set; }
        public double Limit { get; set; }
        public string Currency{ get; set; }
        public string IbanCode { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

    }
}
