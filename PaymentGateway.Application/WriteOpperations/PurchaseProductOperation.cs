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
            Transaction transaction = new Transaction();
            Account account = new Account();

            ProductXTransaction pxt = new ProductXTransaction();
            Product product = database.Products.FirstOrDefault(x => x.Name == operation.Name);
            
            pxt.ProductId = product.Id;
            pxt.TransactionId = transaction.Id;

            if (operation.Value > account.Balance)
            {
                throw new Exception("Fond insuficient");
            }
            if (operation.Limit > product.Limit)
            {
                throw new Exception("Nu avem atatea produse disponibile");
            }

            product.Name = operation.Name;
            product.Limit=operation.Limit ;
            product.Value=operation.Value;
            product.Curency = operation.Curency;

            database.Transactions.Add(transaction);
            database.ProductXTransactions.Add(pxt);

            database.SaveChange();

            ProductPurchased eventProductPurchased = new ProductPurchased()
            {
                Curency = operation.Curency,
                Limit = operation.Limit,
                Name = operation.Name,
                Value = operation.Value
            };
            eventSender.SendEvent(eventProductPurchased);



        }
    }
}
