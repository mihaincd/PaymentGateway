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
    public class CreateProductOperation : IWriteOperations<CreateProductCommand>
    {
        public IEventSender eventSender;
        public CreateProductOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public void PerformOperation(CreateProductCommand operation)
        {
            Database db = Database.GetInstance();

            Product product = new Product();
            product.Curency = operation.Curency;
            product.Limit = operation.Limit;
            product.Name = operation.Name;
            product.Value = operation.Value;

            db.Products.Add(product);
            db.SaveChange();



            //ProductCreated eventProductCreated = new ProductCreated(operation.Name, operation.Value);
            ProductCreated eventProductCreated = new ProductCreated
            {
                Value = operation.Value,
                Name = operation.Name
            };
            eventSender.SendEvent(eventProductCreated);
        }
    }
}
