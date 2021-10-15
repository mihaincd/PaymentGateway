//using Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace PaymentGateway.Application.CommandHandlers
{
    public class CreateProductOperation : IRequestHandler<CreateProductCommand>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public CreateProductOperation(IMediator mediator, Database database)
        {
            _mediator = mediator;
            _database = database;

        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new();
            product.Curency = request.Curency;
            product.Limit = request.Limit;
            product.Name = request.Name;
            product.Value =request.Value;

            _database.Products.Add(product);
            _database.SaveChange();



            //ProductCreated eventProductCreated = new ProductCreated(operation.Name, operation.Value);
            ProductCreated eventProductCreated = new()
            {
                Value = request.Value,
                Name = request.Name
            };
            await _mediator.Publish(eventProductCreated, cancellationToken);

            return Unit.Value;
        }

        
    }
}
