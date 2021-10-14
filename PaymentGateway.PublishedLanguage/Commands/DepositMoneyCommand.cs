using MediatR;
using System;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class DepositMoneyCommand : IRequest
    {
        public int AcountId { get; set; }
        public double DepositAmmount { get; set; }
        public string Curency { get; set; }
        public DateTime DateOfOperation { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}
