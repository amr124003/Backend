using myapp.Data;
using Stripe;
using System;
using myapp.auth.Models;
using Microsoft.Extensions.Configuration;

namespace myapp.auth.Services
{
    public class PaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _stripeSecretKey;

        public PaymentService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _stripeSecretKey = configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        public async Task AddPaymentMethod(Models.PaymentMethod model, string stripeTokenId)
        {
            if (string.IsNullOrEmpty(stripeTokenId))
            {
                throw new ArgumentException("Stripe token ID is required.");
            }

            // Fetch card details from Stripe token to get last 4 digits  
            var tokenService = new TokenService();
            var token = await tokenService.GetAsync(stripeTokenId);
            if (token.Card == null)
            {
                throw new InvalidOperationException("The provided Stripe token does not contain card information.");
            }
            model.LastFourDigits = token.Card.Last4;
            model.ExpiryMonth = token.Card.ExpMonth.ToString();
            model.ExpiryYear = token.Card.ExpYear.ToString();

            _context.PaymentMethods.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Charge> ProcessPayment(decimal amount, string stripeTokenId)
        {
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = (long)(amount * 100), // Amount in cents  
                Currency = "usd",
                Description = "Sample Charge",
                Source = stripeTokenId
            };
            var chargeService = new ChargeService();
            return await chargeService.CreateAsync(chargeOptions);
        }
    }
}
