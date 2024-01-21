using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace Pan.Models.DbClasses
{
    public class Attendees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendeeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string? SecondName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Job { get; set; }
        [MaxLength(150)]
        public string? City { get; set; }
        [MaxLength(150)]
        public string? District { get; set; }
        public string Address { get; set; } = string.Empty;
        public string EmergencyContact { get; set; } = string.Empty;
        public string Descriptions { get; set; } = string.Empty;

        public ICollection<Attendance>? Attendaces { get; set; }

        public ICollection<ClassPayments>? ClassPayments { get; set; }

       
    }
}


