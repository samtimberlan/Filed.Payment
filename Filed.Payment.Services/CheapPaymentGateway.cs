using Filed.Payments.Data.Models;
using Filed.Payments.Data.Models.Results;
using Filed.Payments.Interfaces;
using Filed.Payments.Services;

namespace Filed.Payment.Services
{
    public class CheapPaymentGateway : BaseService, ICheapPaymentGateway
    {
        /// <summary>
        /// Creates a bank transfer using cheap payment gateway
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <returns></returns>
        PaymentResult ICheapPaymentGateway.BankTransfer(PaymentModel paymentModel)
        {
            return SimulateTransferResponse();
        }
    }
}
