using Microsoft.AspNetCore.Mvc;
using myapp.auth.Services;
using myapp.auth.Models;

namespace myapp.auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPaymentMethod([FromBody] AddPaymentMethodRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.StripeTokenId))
                return BadRequest("Invalid request.");

            var paymentMethod = new PaymentMethod
            {
                Type = request.Type,
                CardHolderName = request.CardHolderName,
                StripeTokenId = request.StripeTokenId
            };

            await _paymentService.AddPaymentMethod(paymentMethod, request.StripeTokenId);
            return Ok();
        }
    }

    // DTO for the request
    public class AddPaymentMethodRequest
    {
        public string Type { get; set; }
        public string CardHolderName { get; set; }
        public string StripeTokenId { get; set; }
    }
}
