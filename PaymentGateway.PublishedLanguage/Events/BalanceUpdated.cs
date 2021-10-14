using MediatR;
using System;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class BalanceUpdated :INotification
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
