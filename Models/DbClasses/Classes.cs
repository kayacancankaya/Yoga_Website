using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Models.DbClasses
{ 
    public class Classes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassID { get; set; }

        [Required]
        public int InstructorID { get; set; }
        [Required]
        public int ClassTypeID { get; set; }
        [Required]
        public int ClassMasterID { get; set; }
        [Required]
        public DateTime Schedule { get; set; }
        [Required]
        public int? DurationMinutes { get; set; }
        [Required]
        public int LocationID { get; set; }
        [Required]
        public decimal Price { get; set; }


        [ForeignKey("InstructorID")]
        public Instructors Instructor { get; set; }

        [ForeignKey("ClassTypeID")]
        public ClassTypes ClassType { get; set; }

        [ForeignKey("LocationID")]
        public Locations Locations { get; set; }

        [ForeignKey("ClassID")]
        public ClassPayments ClassPayments { get; set; }
        
        [ForeignKey("ClassID")]

        public ICollection<Attendance> Attendances { get; set; }

    }
}
