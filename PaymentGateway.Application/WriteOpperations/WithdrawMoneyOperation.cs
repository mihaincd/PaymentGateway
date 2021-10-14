using Abstractions;
using PaymentGateway.Application.Services;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using MediatR;

namespace PaymentGateway.Application.WriteOpperations
{
    public class WithdrawMoneyOperation : IWriteOperations<WithdrawMoneyCommand>
    {
        private readonly Mediator _mediator;
        private readonly Database _database;

        public WithdrawMoneyOperation(Mediator mediator, Database database)
        {
            _mediator = mediator;
            _database = database;
        }
        public void PerformOperation(WithdrawMoneyCommand operation)
        {

            Account acount = _database.Accounts.FirstOrDefault(x => x.IdAccount == operation.AcountId);
            Transaction transaction = new Transaction
            {
                Amount = operation.WithdrawAmmount,
                Currency = operation.Curency,
                DateOfTransaction = operation.DateOfTransaction,
                DateOfOperation = DateTime.UtcNow
            };
            var oldAmount = acount.Balance;

            acount.Balance -= operation.WithdrawAmmount;


            _database.Transactions.Add(transaction);
            _database.SaveChange();

            BalanceUpdated eventBalanceUpdated = new BalanceUpdated
            {

                AccountId = acount.IdAccount,
                Curency = acount.Currency,
                EventDate = DateTimeService.GetDate(),
                OldAmount = oldAmount,
                NewAmount = acount.Balance
            };
            _mediator.Send(eventBalanceUpdated);


        }
    }
}

