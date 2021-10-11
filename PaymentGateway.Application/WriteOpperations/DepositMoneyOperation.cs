using Abstractions;
using PaymentGateway.Application.Services;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOpperations
{
    public class DepositMoneyOperation : IWriteOperations<DepositMoneyCommand>
    {
        public IEventSender eventSender;
        public DepositMoneyOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }
        public void PerformOperation(DepositMoneyCommand operation)
        {
            Database database = Database.GetInstance();

            Account acount = database.Accounts.FirstOrDefault(x => x.Id == operation.AcountId);
            Transaction transaction = new Transaction();
            transaction.Amount = operation.DepositAmmount;
            transaction.Currency = operation.Curency;
            transaction.DateOfTransaction = operation.DateOfTransaction;
            transaction.DateOfOperation = DateTime.UtcNow;
            var oldAmount = acount.Balance;

            acount.Balance += operation.DepositAmmount;


            database.Transactions.Add(transaction);
            database.SaveChange();

            BalanceUpdated eventBalanceUpdated = new BalanceUpdated
            {
                AccountId = acount.Id,
                Curency = acount.Currency,
                EventDate = DateTimeService.GetDate(),
                OldAmount = oldAmount,
                NewAmount = acount.Balance
            };
            eventSender.SendEvent(eventBalanceUpdated);

        }
    }
}
