using System.ComponentModel.DataAnnotations;

namespace Pan.Models.DbClasses
{
    public class Events
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [MaxLength(100)]
        public string EventName { get; set; }

        public string Location { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        public float Duration { get; set; }

        public int MaxCapacity { get; set; }

        public string Description { get; set; }


        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public int CreatedBy { get; set; } = 0;
        [Required]
        public DateTime EditedAt { get; set; } = DateTime.Now;
        [Required]
        public int EditedBy { get; set; } = 0;
    }
}

