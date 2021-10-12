using Abstractions;
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
        public CreateAccountOperation(IEventSender eventSender, AccountOptions accountOptions)
        {
            _eventSender = eventSender;
            _accountOptions = accountOptions;
        }
        public void PerformOperation(CreateAccountCommand operation)
        {
            Random random = new Random();

            Database database = Database.GetInstance();
            Person person;
            if (operation.PersonId.HasValue)
            {
                person = database.Persons.FirstOrDefault(x => x.Id == operation.PersonId); //get person by id
            }
            else
            {
                person = database.Persons.FirstOrDefault(x => x.Cnp == operation.Cnp);
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

            Account account = new Account
            {
                Limit = operation.Limit,
                Status = operation.Status,
                Currency = operation.Curency,
                Type = operation.ClientType,
                Balance = operation.Sold,
                IbanCode = string.IsNullOrEmpty(operation.IbanCode) ? random.Next(1000).ToString() : operation.IbanCode,
                Id = database.Persons.Count + 1
            };

            database.Accounts.Add(account);
            database.SaveChange();

            AccountCreated eventAcountCreated = new AccountCreated(operation.Sold, operation.Cnp, operation.Curency);
            _eventSender.SendEvent(eventAcountCreated);

        }
    }
}
