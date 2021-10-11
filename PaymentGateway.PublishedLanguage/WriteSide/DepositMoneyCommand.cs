using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
    public class DepositMoneyCommand
    {
        public int AcountId { get; set; }
        public double DepositAmmount { get; set; }
        public string Curency { get; set; }
        public DateTime DateOfOperation { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}
