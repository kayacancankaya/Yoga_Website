namespace Pan.Models.DbClasses.ViewModels
{
    public class KursGirisViewModel
    {
        public List<string> Locations { get; set; } = new List<string>();
        public List<string> Teachers { get; set; } = new List<string>();
        public List<string> ClassTypes { get; set; } = new List<string>();
        public List<string> Classes { get; set; }
        public Dictionary<int, string> ClassNames { get; set; }
        public string ClassName { get; set; }
        public string ClassTitle { get; set; }
        public string ClassDescription { get; set; }
        public string Instructor { get; set; } = string.Empty;
        public string InstructorFirstName { get; set; } = string.Empty;
        public string? InstructorSecondName { get; set; } = string.Empty;
        public string InstructorLastName { get; set; } = string.Empty;
        public int InstructorID { get; set; }
        public string ClassType { get; set; } = string.Empty;
        public int ClassTypeID { get; set; }
        public DateTime Schedule { get; set; } 
        public List<DateTime> CoursePeriod { get; set; } = new List<DateTime>();
        public string AllCoursePeriod { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string Location { get; set; } = string.Empty;
        public int LocationID { get; set; }
        public int MaxCapacity { get; set; }
        public decimal Price { get; set; }
        public string Level { get; set; } = string.Empty;
        public string RelatedCourses { get; set; } = string.Empty;
        public int ClassMasterID { get; set; }
        

        public List<string> Levels { get; set; }

        public KursGirisViewModel ()
        {
            Levels = new();
            Levels.Add("Başlangıç");
            Levels.Add("Orta");
            Levels.Add("İleri");
            Levels.Add("Tümü");

        }

    }
}
