using System;

namespace Filed.Payments.Data.Entities
{
    public class Payment : BaseEntity
    {
        public string CreditCard { get; set; }
        public string CardHolder { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }
        public PaymentState PaymentState { get; set; }
        public string PaymentReference { get; set; }
    }
}
