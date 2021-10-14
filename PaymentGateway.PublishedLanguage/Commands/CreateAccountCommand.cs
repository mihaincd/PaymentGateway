using MediatR;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class CreateAccountCommand : IRequest
    {
        public int? PersonId { get; set; }
        public double Sold { get; set; }
        public double Limit { get; set; }
        public string IbanCode { get; set; }
        public string Cnp { get; set; }
        public string Curency { get; set; }
        public string ClientType { get; set; }
        public string Status { get; set; }


    }
}
