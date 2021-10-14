﻿using Abstractions;
using PaymentGateway.Application.Services;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace PaymentGateway.Application.WriteOpperations
{
    public class DepositMoneyOperation : IRequestHandler<DepositMoneyCommand>
    {
        private readonly IEventSender _eventSender;
        private readonly Database _database;

        public DepositMoneyOperation(IEventSender eventSender, Database database)
        {
            _eventSender = eventSender;
            _database = database;
        }

        public Task<Unit> Handle(DepositMoneyCommand request, CancellationToken cancellationToken)
        {
            Account acount = _database.Accounts.FirstOrDefault(x => x.Id == request.AcountId);

            Transaction transaction = new Transaction
            {
                Amount = request.DepositAmmount,
                Currency = request.Curency,
                DateOfTransaction = request.DateOfTransaction,
                DateOfOperation = DateTime.UtcNow
            };
            var oldAmount = acount.Balance;

            acount.Balance += request.DepositAmmount;


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
            return Unit.Task;
        }

       
    }
}
