using Filed.Payments.Data.Models.Results;

namespace Filed.Payment.Services.Mappings
{
    public static class PaymentResultMapping
    {
        public static PaymentResult CreateEntity(int statusCode, string message)
        {
            return new PaymentResult
            {
                Message = message,
                StatusCode = statusCode
            };
        }
    }
}
