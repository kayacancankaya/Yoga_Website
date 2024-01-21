using Microsoft.EntityFrameworkCore;
using Pan.Models.DbClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Pan.Models.DbClasses.ViewModels
{
	public class ClassAttendeeViewModel
	{

        public string BeRegisteredAttendee { get; set; }
        [DisplayName("Sınıf Adı")]
        public string? ClassName { get; set; }
        [DisplayName("Sınıf Tipi")]
        public string? ClassType { get; set; }
        [DisplayName("Eğitmen Adı")]
        public string? InstructorName { get; set; }

        public int ClassID { get; set; }
        public int ClassMasterID { get; set; }
        public int AttendeeID { get; set; }

        public DateTime RegistrationDate { get; set;}

        [DisplayName("Mekan")]
        public string? LocationName { get; set; }
        public List<int> ClassIDList { get; set; }
        [DisplayName("Maksimum Kapasite")]
        public int MaxCapacity { get; set; }
        [DisplayName("Mevcut Katılım")]
        public Dictionary<DateTime, int> CurrentAttandence { get; set; }
        [DisplayName("Kayıt Olanlar")]
        public List<Attendees> Registered { get; set; } = new List<Attendees>();
        [DisplayName("Katılımcı Seç")]
        public List<String> Attendees { get; set; } = new List<String>();

        public string CoursesToBeRegistered { get; set; } = string.Empty;
        public List<DateTime> RegisteredCourses { get; set; } = new List<DateTime>();

    }
}
