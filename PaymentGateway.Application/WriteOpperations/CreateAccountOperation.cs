using Abstractions;
using PaymentGateway.Application.ReadOpperations;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Linq;

namespace PaymentGateway.Application.WriteOpperations
{
    public class CreateAccountOperation : IWriteOperations<CreateAccountCommand>
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
        public void PerformOperation(CreateAccountCommand operation)
        {
            Random random = new();

            Person person;
            if (operation.PersonId.HasValue)
            {
                person = _database.Persons.FirstOrDefault(x => x.Id == operation.PersonId); //get person by id
            }
            else
            {
                person = _database.Persons.FirstOrDefault(x => x.Cnp == operation.Cnp);
            }
            if (person == null)
            {
                throw new Exception("Person not found");
            }

            if (operation.ClientType == "Company")
            {
                person.Type = PersonType.Company;
            }
            else if (operation.ClientType == "Individual")
            {
                person.Type = PersonType.Individual;
            }
            else
            {
                throw new Exception("Unsuported person type");
            }

            Account account = new()
            {
                Limit = operation.Limit,
                Status = operation.Status,
                Currency = operation.Curency,
                Type = operation.ClientType,
                Balance = operation.Sold,
                IbanCode = _ibanService.GetNewIban(),
                Id = _database.Persons.Count + 1
            };

            _database.Accounts.Add(account);
            _database.SaveChange();

            AccountCreated eventAcountCreated = new(operation.Sold, operation.Cnp, operation.Curency);
            _eventSender.SendEvent(eventAcountCreated);

        }
    }
}
