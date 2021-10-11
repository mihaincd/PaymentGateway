using Abstractions;
using PaymentGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOpperations
{
    public class MultiplePurchaseOperation : IWriteOperations<MultiplePurchaseOperation>
    {
        public IEventSender eventSender;
        public MultiplePurchaseOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }
        public void PerformOperation(MultiplePurchaseOperation operation)
        {
            Database database = Database.GetInstance();



        }
    }
}
