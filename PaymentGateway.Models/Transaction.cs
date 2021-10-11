using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public DateTime DateOfOperation { get; set; }
        public string Scope { get; set; }
        public string Currency { get; set; }
    }
}
