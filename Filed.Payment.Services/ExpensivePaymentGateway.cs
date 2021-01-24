using Filed.Payments.Data.Models;
using Filed.Payments.Data.Models.Results;
using Filed.Payments.Interfaces;
using Filed.Payments.Services;

namespace Filed.Payment.Services
{
    public class ExpensivePaymentGateway : BaseService, IExpensivePaymentGateway
    {
        /// <summary>
        /// Creates a bank transfer using expensive payment gateway
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <returns></returns>
        public PaymentResult BankTransfer(PaymentModel paymentModel)
        {
            return SimulateTransferResponse();
        }
    }
}
