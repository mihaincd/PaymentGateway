using Abstractions;
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
    public class PurchaseProductOperation : IRequestHandler<PurchaseProductCommand>
    {
        private readonly IEventSender _eventSender;
        private readonly Database _database;

        public PurchaseProductOperation(IEventSender eventSender, Database database)
        {
            _eventSender = eventSender;
            _database = database;
        }

        public Task<Unit> Handle(PurchaseProductCommand request, CancellationToken cancellationToken)
        {
            Account account;
            Person person;

            if (request.IdAccount.HasValue)
            {
                account = _database.Accounts.FirstOrDefault(x => x.IdAccount == request.IdAccount);
            }
            else
            {
                account = _database.Accounts.FirstOrDefault(x => x.IbanCode == request.IbanCode);
            }

            if (request.IdPerson.HasValue)
            {
                person = _database.Persons.FirstOrDefault(x => x.IdProduct == request.IdPerson);
            }
            else
            {
                person = _database.Persons.FirstOrDefault(x => x.Cnp == request.UniqueIdentifier);
            }

            if (account == null) throw new Exception("Account not found");

            if (person == null) throw new Exception("Person not found");

            var exists = _database.Accounts.Any(x => x.IdAccount == person.IdProduct && x.IdAccount == account.IdAccount);

            if (!exists)
            {
                throw new Exception("The person is not associated with the account!");
            }

            var totalAmount = 0d;

            foreach (var item in request.ProductDetails)
            {
                var product = _database.Products.FirstOrDefault(x => x.IdProduct == item.ProductId);

                if (product.Limit < item.Quantity)
                {
                    throw new Exception("Insuficient quantity");
                }

                //product.Limit -= item.Quantity;

                totalAmount += item.Quantity * product.Value;
            }

            if (account.Balance < totalAmount)
            {
                throw new Exception("You have insuficient funds!");
            }

            Transaction transaction = new Transaction
            {
                Amount = -totalAmount
            };

            _database.Transactions.Add(transaction);
            account.Balance -= totalAmount;

            foreach (var item in request.ProductDetails)
            {
                var product = _database.Products.FirstOrDefault(x => x.IdProduct == item.ProductId);
                ProductXTransaction productXTransaction = new ProductXTransaction
                {
                    TransactionId = transaction.IdTransaction,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Value = product.Value,
                    Name = product.Name
                };
            }


            _database.SaveChange();

            ProductPurchased eventProductPurchased = new ProductPurchased()
            {
                ProductDetails = request.ProductDetails
            };
            _eventSender.SendEvent(eventProductPurchased);

            return Unit.Task;
        }


    }
}
