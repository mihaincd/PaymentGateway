using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class BalanceUpdated
    {
        public int AccountId { get; set; }
        public double OldAmount { get; set; }
        public double NewAmount { get; set; }
        public string Curency { get; set; }
        public DateTime EventDate { get; set; } = DateTime.UtcNow;
        public BalanceUpdated()
        {
            EventDate = DateTime.UtcNow;
        }
    }
}
