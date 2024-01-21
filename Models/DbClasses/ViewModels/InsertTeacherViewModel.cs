using System.ComponentModel.DataAnnotations;

namespace Pan.Models.DbClasses.ViewModels
{
    public class InsertTeacherViewModel
    {
        public List<string> ExistingTeachers { get; set; } = new List<string>();
        public Instructors? TeacherToRecord { get; set; }
        public List<string> LocationsToWork { get; set; } = new List<string>();
        public List<string> ClassesToOffer { get; set; } = new List<string>();
        public List<string> EventsToOffer { get; set; } = new List<string>();
        public List<string> ChallengesToOffer { get; set; } = new List<string>();

        public string ParentTeacherName { get; set; } = string.Empty;
        public string StudiosWorkFor { get; set; } = string.Empty;
        public string StudiosOwned { get; set; } = string.Empty;
        public string ClassesGiven { get; set; } = string.Empty;
        public string EventsGiven { get; set; } = string.Empty;
        public string ChallengeGiven { get; set; } = string.Empty;


    }
}
