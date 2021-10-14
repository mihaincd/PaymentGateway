using MediatR;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class ProductCreated : INotification
    {
        public double Value { get; set; }
        public string Name { get; set; }
    }
}
