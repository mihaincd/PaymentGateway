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
    public class WithdrawMoneyOperation : IWriteOperations<WithdrawMoneyCommand>
    {
        private readonly IEventSender _eventSender;
        private readonly Database _database;

        public WithdrawMoneyOperation(IEventSender eventSender, Database database)
        {
            _eventSender = eventSender;
            _database = database;
        }
        public void PerformOperation(WithdrawMoneyCommand operation)
        {

            Account acount = _database.Accounts.FirstOrDefault(x => x.Id == operation.AcountId);
            Transaction transaction = new Transaction();
            transaction.Amount = operation.WithdrawAmmount;
            transaction.Currency = operation.Curency;
            transaction.DateOfTransaction = operation.DateOfTransaction;
            transaction.DateOfOperation = DateTime.UtcNow;
            var oldAmount = acount.Balance;

            acount.Balance -= operation.WithdrawAmmount;


            _database.Transactions.Add(transaction);
            _database.SaveChange();

            BalanceUpdated eventBalanceUpdated = new BalanceUpdated
            {

                AccountId = acount.Id,
                Curency = acount.Currency,
                EventDate = DateTimeService.GetDate(),
                OldAmount = oldAmount,
                NewAmount = acount.Balance
            };
            _eventSender.SendEvent(eventBalanceUpdated);


        }
    }
}

