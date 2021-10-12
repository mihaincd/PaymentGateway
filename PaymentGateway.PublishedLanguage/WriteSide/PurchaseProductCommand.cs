using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
    public class PurchaseProductCommand
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
