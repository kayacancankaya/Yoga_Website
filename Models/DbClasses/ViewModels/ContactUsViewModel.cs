namespace Pan.Models.DbClasses.ViewModels
{
    public class ContactUsViewModel
    {
        public string CompanyAddress { get; set; } = "852. Sok. No.72. D.1. Bornova/İzmir";
        public string PhoneNumber { get; set; } = "+90 555 561 45 47";
        public string Email { get; set; } = "info@panyoga.store";
        public string WorkingHours { get; set; } = "Pazartesi-Cuma 8am-9pm";
        public string YourName { get; set; } = string.Empty;
        public string YourEmail { get; set;} = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<string> SubjectList { get; set; }
        public string Body { get; set; } = string.Empty;   
        
        public ContactUsViewModel() 
        { 
         SubjectList = new ();

            SubjectList.Add("Challenge Bilgi");
            SubjectList.Add("Challenge Şikayet");
            SubjectList.Add ("Etkinlik Bilgi");
            SubjectList.Add ("Etkinlik Şikayet");
            SubjectList.Add("Kurs Bilgi");
            SubjectList.Add("Kurs Şikayet");
            SubjectList.Add("Kusurlu Ürün");
            SubjectList.Add ("Ürün Hakkında Bilgi");
            SubjectList.Add ("Diğer...");

        }
    }
}
