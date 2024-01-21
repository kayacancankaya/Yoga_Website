namespace Pan.Models.DbClasses.ViewModels
{
    public class ClassListsViewModel
    {
        public List<Classes> Classes { get; set; } = new List<Classes>();
        public List<ClassTypes> ClassTypes { get; set; } = new List<ClassTypes>();
        public List<Instructors> Instructors { get; set; } = new List<Instructors>();
        public List<Locations> Locations { get; set; } = new List<Locations>();
        public List<ClassesMaster> ClassesMaster { get; set; } = new List<ClassesMaster>();

        public int NumberOfClassesPerPage { get; set; } = 10;

    }
}
