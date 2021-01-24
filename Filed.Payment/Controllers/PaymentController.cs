using Filed.Payments.Data.Models;
using Filed.Payments.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Filed.Payment.Controllers
{
    //[ApiController]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> ProcessPayment([FromBody]PaymentModel paymentModel)
        {
            var paymentResponse = await _paymentService.ProcessPaymentAsync(paymentModel);
            return StatusCode(paymentResponse.StatusCode, paymentResponse);
        }
    }
}
