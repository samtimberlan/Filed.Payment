using System;
using System.ComponentModel.DataAnnotations;

namespace Filed.Payments.Data.Models
{
    public class PaymentModel
    {
        [CreditCard]
        [Required]
        public string CreditCard { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string CardHolder { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Security code can only contain numbers", MatchTimeoutInMilliseconds = 1000)]
        [StringLength(3, ErrorMessage = "The {0} must be {1} digits long.")]
        public string SecurityCode { get; set; }

        [Range(0, double.PositiveInfinity, ErrorMessage = "{0} must be greater than 0")]
        [Required]
        public decimal Amount { get; set; }
    }
}
