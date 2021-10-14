using Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;

namespace PaymentGateway.Application.WriteOpperations
{
    public class CreateProductOperation : IWriteOperations<CreateProductCommand>
    {
        private readonly IEventSender _eventSender;
        private readonly Database _database;
        public CreateProductOperation(IEventSender eventSender, Database database)
        {
            _eventSender = eventSender;
            _database = database;

        }
        public void PerformOperation(CreateProductCommand operation)
        {

            Product product = new Product();
            product.Curency = operation.Curency;
            product.Limit = operation.Limit;
            product.Name = operation.Name;
            product.Value = operation.Value;

            _database.Products.Add(product);
            _database.SaveChange();



            //ProductCreated eventProductCreated = new ProductCreated(operation.Name, operation.Value);
            ProductCreated eventProductCreated = new ProductCreated
            {
                Value = operation.Value,
                Name = operation.Name
            };
            _eventSender.SendEvent(eventProductCreated);
        }
    }
}
