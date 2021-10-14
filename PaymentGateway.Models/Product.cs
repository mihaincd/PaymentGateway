namespace PaymentGateway.Models
{
    public class Product
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public string Curency { get; set; }
        public double Limit { get; set; }
    }
}
