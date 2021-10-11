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
    public class CreateAccountOperation : IWriteOperations<CreateAccountCommand>
    {
        public IEventSender eventSender;
        public CreateAccountOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;
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

            Account account = new Account();
            account.Limit = operation.Limit;
            account.Status = operation.Status;
            account.Currency = operation.Curency;
            account.Type = operation.ClientType;
            account.Balance = operation.Sold;
            account.IbanCode = string.IsNullOrEmpty(operation.IbanCode) ? random.Next(1000).ToString(): operation.IbanCode  ;
            account.Id = database.Persons.Count + 1;

            database.Accounts.Add(account);
            database.SaveChange();

            AccountCreated eventAcountCreated = new AccountCreated(operation.Sold, operation.Cnp, operation.Curency);
            eventSender.SendEvent(eventAcountCreated);

        }
    }
}
