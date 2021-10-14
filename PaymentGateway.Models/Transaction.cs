using System;

namespace PaymentGateway.Models
{
    public class Transaction
    {
        public int IdTransaction { get; set; }
        public double Amount { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public DateTime DateOfOperation { get; set; }
        public string Scope { get; set; }
        public string Currency { get; set; }
    }
}
