using MediatR;

namespace PaymentGateway.PublishedLanguage.Events
{
    public class AccountCreated : INotification
    {
        public double Sold { get; set; }
        public string Cnp { get; set; }
        public string Curency { get; set; }

        public AccountCreated(double sold, string cnp, string curency)
        {
            Sold = sold;
            Cnp = cnp;
            Curency = curency;
        }
    }
}
