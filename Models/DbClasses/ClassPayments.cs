using System.ComponentModel.DataAnnotations;

namespace Pan.Models.DbClasses
{
    public class ClassPayments
    {
        [Key]
        public int PaymentID { get; set; }

        public int AttendeeID { get; set; }
        public int ClassID { get; set; }

        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? Price3 { get; set; }

        [Required]
        public decimal ActualPrice { get; set; }

        public bool IsPaid { get; set; }

        // Navigation properties to related classes if needed
        public Attendees Attendee { get; set; }
        public Classes Class { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}

