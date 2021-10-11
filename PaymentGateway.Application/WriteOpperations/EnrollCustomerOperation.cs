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

namespace PaymentGateway.Application.WriteOpperations
{
    public class EnrollCustomerOperation : IWriteOperations<EnrollCustomerCommand>
    {
        public IEventSender eventSender;
        public EnrollCustomerOperation(IEventSender eventSender) {
            this.eventSender = eventSender;
        }
        public void PerformOperation(EnrollCustomerCommand operation)
        {
            Database database = Database.GetInstance();
            Person person = new Person();
            Random random = new Random();

            person.Cnp = operation.UniqueIdentifier;
            person.Name = operation.Name;
            if(operation.ClientType == "Company")
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
            person.Id = database.Persons.Count + 1;
            database.Persons.Add(person);

            Account account = new Account();
            account.Type = operation.AccountType;
            account.Currency = operation.Currency;
            account.IbanCode = random.Next(100000).ToString();

            database.Accounts.Add(account);
            database.SaveChange();

            CustomerEnrolled eventCustomerEnroll = new CustomerEnrolled(operation.Name, operation.UniqueIdentifier, operation.ClientType);
            eventSender.SendEvent(eventCustomerEnroll);

        }
    }
}
