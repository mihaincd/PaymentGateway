using Abstractions;
using PaymentGateway.Application.ReadOpperations;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace PaymentGateway.Application.WriteOpperations
{
    public class EnrollCustomerOperation : IRequestHandler<EnrollCustomerCommand>
    {
        private readonly IEventSender _eventSender;
        private readonly Database _database;
        private readonly NewIban _ibanService;

        public EnrollCustomerOperation(IEventSender eventSender, Database database, NewIban ibanService)
        {
            _eventSender = eventSender;
            _database = database;
            _ibanService = ibanService;
        }

        public Task<Unit> Handle(EnrollCustomerCommand request, CancellationToken cancellationToken)
        {
            Person person = new()
            {
                Cnp = request.UniqueIdentifier,
                Name = request.Name
            };
            if (request.ClientType == "Company")
            {
                person.Type = PersonType.Company;
            }
            else if (request.ClientType == "Individual")
            {
                person.Type = PersonType.Individual;
            }
            else
            {
                throw new Exception("Unsuported person type");
            }
            person.Id = _database.Persons.Count + 1;
            _database.Persons.Add(person);

            Account account = new()
            {
                Type = request.AccountType,
                Currency = request.Currency,
                IbanCode = _ibanService.GetNewIban()
            };

            _database.Accounts.Add(account);
            _database.SaveChange();

            CustomerEnrolled eventCustomerEnroll = new(request.Name, request.UniqueIdentifier, request.ClientType);
            _eventSender.SendEvent(eventCustomerEnroll);

            return Unit.Task;
        }

        
    }
}
