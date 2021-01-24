namespace Filed.Payments.Data.Entities
{
    public class PaymentState : BaseEntity
    {
        public long PaymentId { get; set; }
        public string PaymentReference { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
