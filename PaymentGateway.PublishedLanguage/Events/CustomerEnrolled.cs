using MediatR;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class CustomerEnrolled : INotification
    {
        public string Name { get; set; }
        public string UniqueIdentifier { get; set; }
        public string ClientType { get; set; }

        public CustomerEnrolled(string name, string cnp, string clientType) {
            Name = name;
            UniqueIdentifier = cnp;
            ClientType = clientType;
        }
    }
}
