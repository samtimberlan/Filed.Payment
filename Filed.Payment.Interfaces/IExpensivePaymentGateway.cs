using Filed.Payments.Data.Models;
using Filed.Payments.Data.Models.Results;

namespace Filed.Payments.Interfaces
{
    public interface IExpensivePaymentGateway
    {
        /// <summary>
        /// Creates a bank transfer using expensive payment gateway
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <returns></returns>
        PaymentResult BankTransfer(PaymentModel paymentModel);
    }
}
