using System.ComponentModel.DataAnnotations;

namespace Pan.Models.DbClasses
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }

        public int ClassID { get; set; }
        public int AttendeeID { get; set; }

        public DateTime? CheckInTime { get; set; }

        // Navigation properties to related classes if needed
        public Classes Class { get; set; }
        public Attendees Attendee { get; set; }

    }
}
