namespace Pan.Models.PanelClasses
{
    public class Cls_Panel
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Departman { get; set; }

        public string ImageName { get; }
        public string AdSoyad { get; }
        public Cls_Panel()
        {
        }
        public Cls_Panel(string ad, string soyad, string departman)
        {
            Ad = ad;
            Soyad = soyad;
            ImageName = $"{ad}_{soyad}.jpg";
            AdSoyad = $"{ad} {soyad}";
            Departman = departman;
        }
    }
}
