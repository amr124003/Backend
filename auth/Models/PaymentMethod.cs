namespace myapp.auth.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string CardHolderName { get; set; }
        public string LastFourDigits { get; set; }
        public string ExpiryMonth { get; set; }
        public string  ExpiryYear { get; set; }
        public string  StripeTokenId { get; set; }
    }
}