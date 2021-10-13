using System;
using Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.WriteSide;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.Application.ReadOpperations;

namespace PaymentGateway.Application.WriteOpperations
{
    public class EnrollCustomerOperation : IWriteOperations<EnrollCustomerCommand>
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


        public void PerformOperation(EnrollCustomerCommand operation)
        {
            Person person = new()
            {
                Cnp = operation.UniqueIdentifier,
                Name = operation.Name
            };
            if (operation.ClientType == "Company")
            {
                person.Type = PersonType.Company;
            }else if(operation.ClientType == "Individual")
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
                Type = operation.AccountType,
                Currency = operation.Currency,
                IbanCode = _ibanService.GetNewIban()
            };

            _database.Accounts.Add(account);
            _database.SaveChange();

            CustomerEnrolled eventCustomerEnroll = new(operation.Name, operation.UniqueIdentifier, operation.ClientType);
            _eventSender.SendEvent(eventCustomerEnroll);

        }
    }
}
