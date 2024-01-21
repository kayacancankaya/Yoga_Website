using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Models.DbClasses
{
	public class Registrations
	{
		[Key]
		public int RegistrationID { get; set; }

		[ForeignKey("Classes")]
		public int ClassID { get; set; }

		[ForeignKey("Attendees")]
		public int AttendeeID { get; set; }

		public DateTime RegistrationTime { get; set; }

		// Navigation properties
		public virtual Classes Class { get; set; }

		public virtual Attendees Attendee { get; set; }
	}
}
