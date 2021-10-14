using Abstractions;
using MediatR;
using PaymentGateway.Application.ReadOpperations;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOpperations
{
    public class CreateAccountOperation : IRequestHandler<CreateAccountCommand>
    {
        private readonly IEventSender _eventSender;
        private readonly AccountOptions _accountOptions;
        private readonly Database _database;
        private readonly NewIban _ibanService;
        public CreateAccountOperation(IEventSender eventSender, AccountOptions accountOptions, Database database, NewIban ibanService)
        {
            _eventSender = eventSender;
            _accountOptions = accountOptions;
            _database = database;
            _ibanService = ibanService;
        }

        public Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            Random random = new();

            Person person;
            if (request.PersonId.HasValue)
            {
                person = _database.Persons.FirstOrDefault(x => x.Id == request.PersonId); //get person by id
            }
            else
            {
                person = _database.Persons.FirstOrDefault(x => x.Cnp == request.Cnp);
            }
            if (person == null)
            {
                throw new Exception("Person not found");
            }

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

            Account account = new()
            {
                Limit = request.Limit,
                Status = request.Status,
                Currency = request.Curency,
                Type = request.ClientType,
                Balance = request.Sold,
                IbanCode = _ibanService.GetNewIban(),
                Id = _database.Persons.Count + 1
            };

            _database.Accounts.Add(account);
            _database.SaveChange();

            AccountCreated eventAcountCreated = new(request.Sold, request.Cnp, request.Curency);
            _eventSender.SendEvent(eventAcountCreated);

            return Unit.Task;

        }

  
    }
}
