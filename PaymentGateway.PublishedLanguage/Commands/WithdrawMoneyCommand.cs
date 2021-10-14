using MediatR;
using System;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class WithdrawMoneyCommand : IRequest
    {
        public int AcountId { get; set; }
        public double WithdrawAmmount { get; set; }
        public string Curency { get; set; }
        public DateTime DateOfOperation { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}
