using System.ComponentModel.DataAnnotations;

namespace Pan.Models.DbClasses
{
    public class EventPayments
    {
        [Key]
        public int EventPaymentID { get; set; }

        public int AttendeeID { get; set; }

        [Required]
        public decimal PaymentAmount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        // You can add navigation properties if needed
    }
}

