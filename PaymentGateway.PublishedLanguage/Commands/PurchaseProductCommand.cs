using MediatR;
using System.Collections.Generic;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class PurchaseProductCommand : IRequest
    {
        public List<PurchaseProductDetail> ProductDetails = new List<PurchaseProductDetail>();
        public string UniqueIdentifier { get; set; }
        public double Value { get; set; }
        public int? IdPerson { get; set; }
        public string IbanCode { get; set; }
        public int? IdAccount { get; set; }
        

        public class PurchaseProductDetail
        {
            public int ProductId { get; set; }
            public double Quantity { get; set; }
        }

    }
}
