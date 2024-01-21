namespace Pan.Models.DbClasses.ViewModels
{
    public class InsertCoursesPicturesAndLinksViewModel
    {
        public Dictionary<string, string> PhotoNames { get; set; }
        public Dictionary<int, string> CourseInfo { get; set; }

        public string VideoPath { get; set; }
        public InsertCoursesPicturesAndLinksViewModel()
        {
            PhotoNames = new();
            PhotoNames.Add("Kurs Fotosu", "800*624");
            PhotoNames.Add("Galeri Fotosu 1.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 2.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 3.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 4.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 5.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 6.Resim", "360*286");
        }
    }
}
