using Filed.Payments.Data.Entities;
using Filed.Payment.Services.Extension_Methods;
using Filed.Payments.Data;

namespace Filed.Payment.Services.Mappings
{
    internal static class PaymentStateMapping
    {
        public static PaymentState CreateEntity(Payments.Data.Entities.Payment payment, PaymentStatus paymentStatus, string username, string userId)
        {
            var paymentState = new PaymentState
            {
                PaymentStatus = paymentStatus,
                PaymentReference = payment.PaymentReference
            };

            paymentState.SetCreatedFields(username, userId:userId);

            // Add newly created payment state to payment
            payment.PaymentState = paymentState;

            return paymentState;
        }
    }
}
