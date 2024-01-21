namespace Pan.Models.DbClasses.ViewModels
{
    public class InsertTeacherPicturesAndLinksViewModel
    {
        public Dictionary<string,string>PhotoNames { get; set; }
        public Dictionary<int,string>InstructorInfo { get; set; }
        public List<string> Links { get; set; }
        public InsertTeacherPicturesAndLinksViewModel()
        {
            PhotoNames = new ();
            PhotoNames.Add("Panel Fotosu", "160*160");
            PhotoNames.Add("Takım Sayfası Fotosu", "458*494");
            PhotoNames.Add("Hakkımda Sayfası Ana Foto", "1920*910");
            PhotoNames.Add("Hakkımda Sayfası 1.Resim", "960*650");
            PhotoNames.Add("Hakkımda Sayfası 2.Resim", "960*650");
            PhotoNames.Add("Galeri Fotosu 1.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 2.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 3.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 4.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 5.Resim", "360*286");
            PhotoNames.Add("Galeri Fotosu 6.Resim", "360*286");
            PhotoNames.Add("Bahsedildi İkon 1.Resim", "253*182");
            PhotoNames.Add("Bahsedildi İkon 2.Resim", "253*182");
            PhotoNames.Add("Bahsedildi İkon 3.Resim", "253*182");
            PhotoNames.Add("Bahsedildi İkon 4.Resim", "253*182");
            PhotoNames.Add("Bahsedildi İkon 5.Resim", "253*182");
            Links = new ();
            Links.Add("Instagram");
            Links.Add("Facebook");
            Links.Add("Twitter");
            Links.Add("Bahsedildi1");
            Links.Add("Bahsedildi2");
            Links.Add("Bahsedildi3");
            Links.Add("Bahsedildi4");
            Links.Add("Bahsedildi5");
        }
    }

}
