using Abstractions;
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
    public class PurchaseProductOperation : IWriteOperations<PurchaseProductCommand>
    {
        public IEventSender eventSender;
        public PurchaseProductOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public void PerformOperation(PurchaseProductCommand operation)
        {
            Database database = Database.GetInstance();
            Account account;
            Person person;

            if (operation.IdAccount.HasValue)
            {
                account = database.Accounts.FirstOrDefault(x => x.Id == operation.IdAccount);
            }
            else
            {
                account = database.Accounts.FirstOrDefault(x => x.IbanCode == operation.IbanCode);
            }

            if (operation.IdPerson.HasValue)
            {
                person = database.Persons.FirstOrDefault(x => x.Id == operation.IdPerson);
            }
            else
            {
                person = database.Persons.FirstOrDefault(x => x.Cnp == operation.UniqueIdentifier);
            }

            if (account == null) throw new Exception("Account not found");

            if (person == null) throw new Exception("Person not found");

            var exists = database.Accounts.Any(x => x.Id == person.Id && x.Id == account.Id);

            if (!exists)
            {
                throw new Exception("The person is not associated with the account!");
            }

            var totalAmount = 0d;

            foreach (var item in operation.ProductDetails)
            {
                var product = database.Products.FirstOrDefault(x => x.Id == item.ProductId);

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

            Transaction transaction = new Transaction();
            transaction.Amount = -totalAmount;
            database.Transactions.Add(transaction);
            account.Balance -= totalAmount;

            foreach (var item in operation.ProductDetails)
            {
                var product = database.Products.FirstOrDefault(x => x.Id == item.ProductId);
                ProductXTransaction productXTransaction = new ProductXTransaction();
                productXTransaction.TransactionId = transaction.Id;
                productXTransaction.ProductId = item.ProductId;
                productXTransaction.Quantity = item.Quantity;
                productXTransaction.Value = product.Value;
                productXTransaction.Name = product.Name;
            }


            database.SaveChange();

            ProductPurchased eventProductPurchased = new ProductPurchased()
            {
                ProductDetails = operation.ProductDetails
            };
            eventSender.SendEvent(eventProductPurchased);

        }
    }
}
