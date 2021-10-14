using MediatR;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class CreateProductCommand : IRequest
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public string Curency { get; set; }
        public double Limit { get; set; }

        }
}
