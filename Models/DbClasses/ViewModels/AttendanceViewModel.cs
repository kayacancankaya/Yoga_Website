namespace Pan.Models.DbClasses.ViewModels
{
    public class AttendanceViewModel
    {
        public List<Attendance> Attendances { get; set; }
        public Classes ClassInfo { get; set; }
        public ClassesMaster ClassInfoMaster { get; set; }
        public List<Attendees> Students { get; set; }

        public List<ClassPayments> Payments { get; set; }

        public List<Attendees> Registrars { get; set; }
    }
}
