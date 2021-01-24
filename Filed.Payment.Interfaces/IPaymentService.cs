using Filed.Payments.Data.Models;
using Filed.Payments.Data.Models.Results;
using System.Threading.Tasks;

namespace Filed.Payments.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Processes a payment request
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <returns></returns>
        Task<IExecutionResponse<PaymentResult>> ProcessPaymentAsync(PaymentModel paymentModel);
    }
}
