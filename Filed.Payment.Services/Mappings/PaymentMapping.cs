using Filed.Payment.Services.Extension_Methods;
using Filed.Payments.Data.Models;
using System;

namespace Filed.Payment.Services.Mappings
{
    public static class PaymentMapping
    {
        public static Payments.Data.Entities.Payment CreateEntity(PaymentModel paymentModel, string userName, string userId)
        {
            var payment = new Payments.Data.Entities.Payment
            {
                Amount = paymentModel.Amount,
                CardHolder = paymentModel.CardHolder,
                CreditCard = paymentModel.CreditCard,
                ExpirationDate = paymentModel.ExpirationDate,
                PaymentReference = "FP" + Guid.NewGuid().ToString(),
                SecurityCode = paymentModel.SecurityCode
            };
            payment.SetCreatedFields(userName, userId);

            return payment;
        }
    }
}
