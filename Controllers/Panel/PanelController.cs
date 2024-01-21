using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pan.Models;
using Pan.Models.DbClasses.ViewModels;
using Pan.Models.DbClasses;
using Pan.Models.PanelClasses;
using System.Drawing;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;

namespace Pan.Controllers.Panel
{
	public class PanelController : Controller
    {
        private readonly PanDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        bool result = false;
        int counter = 0;
        public PanelController(PanDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            Cls_Panel panel = new Cls_Panel("Yasemin", "Kirpikli", "Yönetim");
         
            return View(panel);
        }

        public ActionResult KursListesi()
        {
            ClassListsViewModel classList = new();

            classList.Classes = _context.Classes.Where(s => s.Schedule > DateTime.Now).ToList();

           
            foreach (Classes item in classList.Classes) 
            {
                classList.ClassesMaster.Add(_context.ClassesMaster.FirstOrDefault(i=>i.ClassMasterID == item.ClassMasterID)); 
                classList.ClassTypes.Add(_context.ClassTypes.FirstOrDefault(i=>i.ClassTypeID == item.ClassTypeID)); 

                classList.Instructors.Add(_context.Instructors.FirstOrDefault(i => i.InstructorID == item.InstructorID)); 
                classList.Locations.Add(_context.Locations.FirstOrDefault(i => i.LocationID == item.LocationID)); 
            }

            return View(classList);

        }

        public ActionResult TamamlananKursListesi()
        {
            ClassListsViewModel classList = new();

            classList.Classes = _context.Classes.Where(s => s.Schedule > DateTime.Now).ToList();


            foreach (Classes item in classList.Classes)
            {
                classList.ClassesMaster.Add(_context.ClassesMaster.FirstOrDefault(i => i.ClassMasterID == item.ClassMasterID));
                classList.ClassTypes.Add(_context.ClassTypes.FirstOrDefault(i => i.ClassTypeID == item.ClassTypeID));

                classList.Instructors.Add(_context.Instructors.FirstOrDefault(i => i.InstructorID == item.InstructorID));
                classList.Locations.Add(_context.Locations.FirstOrDefault(i => i.LocationID == item.LocationID));
            }

            return View(classList);
        }
       
        public ActionResult KursKatilimListesi(int id)
        {
            var attendance = _context.Attendance.Where(c => c.ClassID == id).ToList();
            if (attendance == null)
                return RedirectToAction("NotFound");

            var class_ = _context.Classes.Include(c => c.Instructor).Include(c => c.ClassType)
                          .Include(c => c.Locations).FirstOrDefault(c => c.ClassID == id);

            var classMaster = _context.ClassesMaster.Where(i=> i.ClassMasterID == class_.ClassMasterID).FirstOrDefault();  

            var studentsList = attendance.Select(item => item.AttendeeID).ToList();

            var attendees = _context.Attendees.Where(s => studentsList.Contains(s.AttendeeID)).ToList();

            var payments = _context.ClassPayments.Where(p => p.ClassID == id).ToList();

            var registrations = _context.Registrations.Select(r=>r.ClassID).ToList();

            var registrars = _context.Attendees.Where(r => registrations.Contains(r.AttendeeID)).ToList();

            var attendanceViewModel = new AttendanceViewModel
            {
                Attendances = attendance,
                ClassInfo = class_,
                ClassInfoMaster = classMaster,
                Students = attendees,
                Payments = payments,
                Registrars = registrars,
            };
            return View(attendanceViewModel);
        }

        [HttpPost]
        public ActionResult KursSil(int id)
        {
            var course = _context.Classes.Where(c => c.ClassID == id).FirstOrDefault();

            var masterID = _context.Classes.Where(c=>c.ClassID == id).Select(i=>i.ClassMasterID).FirstOrDefault();

            var payments = _context.ClassPayments.Where(p => p.ClassID == id).ToList();

            var result = new
            {
                success = true,
                message = "Kurs silindi" 
            };


            if (course == null)
            {
                TempData["ErrorMessage"] = "Kurs Bulunamadı.";
                return RedirectToAction("KursListesi", new { deleteResult = "failed no course" });
            }

            // Check if there are paid students
            if (payments.Any(p => p.IsPaid))
            {
                TempData["ErrorMessage"] = "Ödeme Mevcut Kurs Silinemedi.";
                return RedirectToAction("KursListesi", new { deleteResult = "failed paid" });
            }
            else
            {
                var attendanceRecords = _context.Attendance.Where(a => a.ClassID == id);
                if (attendanceRecords.Any())
                {
                    _context.Attendance.RemoveRange(attendanceRecords);
                }

                foreach (var entry in payments)
                {
                    _context.ClassPayments.Remove(entry);
                }

                _context.Classes.Remove(course);

                var ifOtherClassesExists = _context.Classes.Select(i => i.ClassMasterID == masterID).Any();
                if (!ifOtherClassesExists) 
                {
                    ClassesMaster masterClassToRemove = _context.ClassesMaster.Where(i => i.ClassMasterID == masterID).FirstOrDefault();
                    _context.ClassesMaster.Remove(masterClassToRemove);
                }
                try { 
                _context.SaveChanges();
                }
                catch 
                {
                    TempData["ErrorMessage"] = "Kayıt Silinirken Hata İle Karşılaşıldı.";
                    return RedirectToAction("KursListesi", new { deleteResult = "failed database" });
                }
                TempData["SuccessMessage"] = "Kurs silindi";
                return RedirectToAction("KursListesi", new { deleteResult = "success" });

            }

        }

        public ActionResult KursGiris()
        {
            try
            {
            var locations = _context.Locations
                            .Where(item => item.LocationTypeID == 1 && item.Open_ == true).Select(item => item.LocationName).Distinct().ToList();
            var teachers = _context.Instructors
                .Select(item => item.FirstName + " " + " " + item.SecondName + " " + item.LastName)
                .Distinct()
                .ToList();
            var classTypes = _context.ClassTypes.Select(item => item.ClassName).Distinct().ToList();

            Dictionary<int, string> classNames = _context.ClassesMaster.ToDictionary(n => n.ClassMasterID, n => n.ClassName);

            var kursGirisViewModel = new KursGirisViewModel
            {
                Locations = locations,
                Teachers = teachers,
                ClassTypes = classTypes,
                ClassNames = classNames,

            };
            return View(kursGirisViewModel);


            }
            catch
            {
                return RedirectToAction("NotFound");
            }
        }

        [HttpPost]
        public ActionResult InsertKursData(KursGirisViewModel kursGirisViewModel)
        {
            
            List<DateTime> coursePeriodList = new List<DateTime>();
            //course schedule
            if (!string.IsNullOrEmpty(kursGirisViewModel.AllCoursePeriod))
            {
                // Split the string by semicolons to get individual date components
                string[] dateComponents = kursGirisViewModel.AllCoursePeriod.Split(',');

                
                foreach (string dateComponent in dateComponents)
                {
                    string trimmedDateComponent = dateComponent.Trim();

                    int year = int.Parse(trimmedDateComponent.Substring(0, 4));
                    int month = int.Parse(trimmedDateComponent.Substring(4, 2));
                    int day = int.Parse(trimmedDateComponent.Substring(6, 2));
                    int hour = int.Parse(trimmedDateComponent.Substring(8, 2));
                    int minute = int.Parse(trimmedDateComponent.Substring(10, 2));
                    
                    DateTime parsedDate = new DateTime(year, month, day, hour, minute, 0);
                    coursePeriodList.Add(parsedDate);
                }
            }

            kursGirisViewModel.CoursePeriod = coursePeriodList;

            //instructurID
            string[] nameParts = kursGirisViewModel.Instructor.Split(' ');

            string firstName = string.Empty;
            string secondName = string.Empty;
            string lastName = string.Empty;

            if (nameParts.Length == 2)
            {
                firstName = nameParts[0];
                lastName = nameParts[1];


                kursGirisViewModel.InstructorID = _context.Instructors
                    .Where(instructor => instructor.FirstName == firstName && instructor.LastName == lastName)
                    .Select(id => id.InstructorID)
                    .FirstOrDefault();
            }
            if (nameParts.Length == 3)
            {
                firstName = nameParts[0];
                secondName = nameParts[1];
                lastName = nameParts[1];


                kursGirisViewModel.InstructorID = _context.Instructors
                    .Where(instructor => instructor.FirstName == firstName 
                            && instructor.SecondName == secondName
                            && instructor.LastName == lastName)
                    .Select(id => id.InstructorID)
                    .FirstOrDefault();
            }

            //location
            kursGirisViewModel.LocationID = _context.Locations.Where(location => location.LocationName == kursGirisViewModel.Location)
                                                              .Select(location => location.LocationID)
                                                              .FirstOrDefault();   
            //class type
            kursGirisViewModel.ClassTypeID = _context.ClassTypes.Where(type => type.ClassName == kursGirisViewModel.ClassType)
                                                              .Select(type => type.ClassTypeID)
                                                              .FirstOrDefault();
            ClassesMaster classMaster = new ClassesMaster
            {
                ClassName = kursGirisViewModel.ClassName,
                InstructorID = kursGirisViewModel.InstructorID,
                ClassTypeID = kursGirisViewModel.ClassTypeID,
                LocationID = kursGirisViewModel.LocationID,
                MaxCapacity = kursGirisViewModel.MaxCapacity,
                TotalPrice = kursGirisViewModel.Price * kursGirisViewModel.CoursePeriod.Count(),//0 geldi kontrol
                Level = kursGirisViewModel.Level,
                Related = kursGirisViewModel.RelatedCourses,
                Popularity = 0,
                ClassTitle = kursGirisViewModel.ClassTitle,
                Description = kursGirisViewModel.ClassDescription,
                VideoPath = string.Empty,
                Photopath1 = string.Empty,
                Photopath2 = string.Empty,
                Photopath3 = string.Empty,
                Photopath4 = string.Empty,
                Photopath5 = string.Empty,
                Photopath6 = string.Empty,
                Photopath7 = string.Empty,
                Photopath8 = string.Empty,
                Photopath9 = string.Empty,
                

            };
            try
            {
                _context.ClassesMaster.Add(classMaster);
                _context.SaveChanges();
            }
            catch
            {

                return Json(new { success = false });
            }


            kursGirisViewModel.ClassMasterID = _context.ClassesMaster.OrderBy(i => i.ClassMasterID).Select(i=>i.ClassMasterID).LastOrDefault();

            if (kursGirisViewModel.ClassMasterID == null)
            {
                //delete possible record
                    try
                    {
                    _context.ClassesMaster.Remove(classMaster);
                    _context.SaveChanges();
                    }
                    catch
                    {
                        return Json(new { success = false });
                    }
                
                return Json(new { success = false }); 
            
            }

            foreach (var scheduleDate in kursGirisViewModel.CoursePeriod )
            {
                Classes classes = new Classes
                {
                    ClassMasterID = kursGirisViewModel.ClassMasterID,
                    InstructorID = kursGirisViewModel.InstructorID,
                    ClassTypeID = kursGirisViewModel.ClassTypeID,
                    LocationID = kursGirisViewModel.LocationID,
                    Schedule = scheduleDate,
                    DurationMinutes = kursGirisViewModel.DurationMinutes,
                    Price = kursGirisViewModel.Price,
                };
                try
                {

                    _context.Classes.Add(classes);
                    _context.SaveChanges();
                }
                catch 
                {
                    List <Classes> classesToRemove = _context.Classes.Where(i=>i.ClassMasterID ==  kursGirisViewModel.ClassMasterID).ToList();
                    if (classesToRemove.Count > 0) 
                    { 
                        foreach (Classes item in classesToRemove)
                            _context.Remove(item);
                    }
                    ClassesMaster classMastertoRemove = _context.ClassesMaster.Where(i => i.ClassMasterID == kursGirisViewModel.ClassMasterID).FirstOrDefault();
                    _context.Remove(classMastertoRemove);

                    return Json(new { success = false });
                }

            }

            return Json(new { success = true });

        }
        
        public ActionResult InsertPhotosAndLinksForCourses()
        { 
            try 
	        {
                
		        return View();
	        }
	        catch
	        {
               return RedirectToAction("NotFound");
	        }
        }

        public ActionResult KursOgrenciKayit()
        {

            Attendees attendees = new();

            return View(attendees);
        }

        [HttpPost]
        public ActionResult InsertStudentData(Attendees attendee)
        {

          
                try
                {
                    var attendeeAlreadyExists = _context.Attendees.Where(a => a.FirstName == attendee.FirstName && a.LastName == attendee.LastName).Any();   
                    if (!attendeeAlreadyExists)
                    {
                        _context.Attendees.Add(attendee);
                        _context.SaveChanges();
                    }
                    if(attendeeAlreadyExists)
                    {
                        return Json(new { success = false, message = "Katılımcı Sistemde Mevcut." });
                    }

                }
                catch (Exception)
                {

                    return Json(new { success = false, message = "Katılımcı Kaydedilirken Hata İle Karşılaşıldı." });
                }

            return Json(new { success = true, message="Katılımcı Başarıyla Kaydedildi." });

        }

        public async Task<IActionResult> KatilimciKursKaydi(int classID)
        {
            var classInfo = await _context.Classes
                                    .Where(c => c.ClassID == classID)
                                    .FirstOrDefaultAsync();

            if(classInfo == null)
                return Json(new { success = false, message = "Kurs Bulunamadı!" });

            string? className = await _context.ClassesMaster.Where(t => t.ClassMasterID == classInfo.ClassMasterID).Select(t=>t.ClassName).FirstOrDefaultAsync();
            int classMasterID = await _context.ClassesMaster.Where(t => t.ClassMasterID == classInfo.ClassMasterID).Select(t => t.ClassMasterID).FirstOrDefaultAsync();
            string? classType = await _context.ClassTypes.Where(t => t.ClassTypeID == classInfo.ClassTypeID).Select(t=>t.ClassName).FirstOrDefaultAsync();
           
            string? teacher = await _context.Instructors.Where(i => i.InstructorID == classInfo.InstructorID).Select(i => string.Join(" ", i.FirstName, i.SecondName, i.LastName).Trim()).FirstOrDefaultAsync();

			string? place = await _context.Locations.Where(l => l.LocationID == classInfo.LocationID).Select(l => l.LocationName).FirstOrDefaultAsync();

            int maxCapacity = await _context.ClassesMaster.Where(i=>i.ClassMasterID == classInfo.ClassMasterID).Select(m=>m.MaxCapacity).FirstOrDefaultAsync();

            var classInfoDetailed = await _context.Classes.Where(c => c.ClassID == classID).ToListAsync();

            List<Registrations> registries = new();
            List<Attendees> registered = new();
            Dictionary<DateTime,int> currentAttendance = new();
            DateTime schedule;
            int activeAttendance;
            foreach (var id in classInfoDetailed) 
            { 
                var registration = await _context.Registrations.Where(c => c.ClassID == id.ClassID).FirstOrDefaultAsync();
                if (registration != null)
                {
                    registries.Add(registration);
                    var attendee = await _context.Attendees.Where(r => r.AttendeeID == registration.AttendeeID).FirstOrDefaultAsync();
                    if (attendee != null)
                        registered.Add(attendee);
                }
                schedule = await _context.Classes.Where(i => i.ClassID == id.ClassID).Select(s => s.Schedule).FirstOrDefaultAsync();
                activeAttendance = await _context.Registrations.Where(c => c.ClassID == id.ClassID).CountAsync();
                currentAttendance.Add(schedule,activeAttendance);
            }

            registered.OrderBy(r => r.FirstName).ThenBy(r => r.LastName);

            var attendees = await _context.Attendees.OrderBy(a=>a.FirstName).ThenBy(a=>a.LastName).Select(a => string.Join(" ", a.FirstName, a.SecondName, a.LastName).Trim()).ToListAsync();

            var classAttendeeViewModel = new ClassAttendeeViewModel
            {
                ClassName = className,
                ClassType = classType,
                InstructorName = teacher,
                LocationName = place,
                MaxCapacity = maxCapacity,
                Registered = registered,
                Attendees = attendees,
                CurrentAttandence = currentAttendance,
                ClassMasterID = classMasterID,
                ClassID = classID,
            };

			return View(classAttendeeViewModel);
        }

        [HttpPost]
        public ActionResult InsertKursKatimciKayit(ClassAttendeeViewModel classAttendeeViewModel)
        {

            try
            {
                List<DateTime> registeredCoursesList = new List<DateTime>();

                if (string.IsNullOrEmpty(classAttendeeViewModel.CoursesToBeRegistered))
                    return Json(new { success = false, message = "Kaydedilecek Kurs Tarihi Bulunamadı." });
                

                    // Split the string by semicolons to get individual date components
                    string[] dateComponents = classAttendeeViewModel.CoursesToBeRegistered.Split(',');


                    foreach (string dateComponent in dateComponents)
                    {
                    
                        int year = int.Parse(dateComponent.Substring(0, 4));
                        int month = int.Parse(dateComponent.Substring(5, 2));
                        int day = int.Parse(dateComponent.Substring(8, 2));
                        int hour = int.Parse(dateComponent.Substring(11, 2));
                        int minute = int.Parse(dateComponent.Substring(14, 2));

                        DateTime parsedDate = new DateTime(year, month, day, hour, minute, 0);
                        registeredCoursesList.Add(parsedDate);
                    }
                

                classAttendeeViewModel.RegisteredCourses = registeredCoursesList;

                string[] nameParts = classAttendeeViewModel.BeRegisteredAttendee.Split(' ');
                
                string firstName=string.Empty;
                string secondName=string.Empty;
                string lastName=string.Empty;


                if (nameParts.Length == 2)
                {
                    firstName = nameParts[0];
                    lastName = nameParts[1];


                    classAttendeeViewModel.AttendeeID = _context.Attendees
                        .Where(att => att.FirstName == firstName && att.LastName == lastName)
                        .Select(id => id.AttendeeID)
                        .FirstOrDefault();
                }
                if (nameParts.Length == 3)
                {
                    firstName = nameParts[0];
                    secondName = nameParts[1];
                    lastName = nameParts[2];


                    classAttendeeViewModel.AttendeeID = _context.Attendees
                        .Where(att => att.FirstName == firstName && att.LastName == lastName)
                        .Select(id => id.AttendeeID)
                        .FirstOrDefault();
                }
                foreach (var registeredCourse in classAttendeeViewModel.RegisteredCourses)
                {
                    Registrations registration = new Registrations
                    {
                        ClassID = classAttendeeViewModel.ClassID,
                        AttendeeID = classAttendeeViewModel.AttendeeID,

                    };

                    if(_context.Registrations.Where(c=>c.ClassID == registration.ClassID).Where(a => a.AttendeeID == classAttendeeViewModel.AttendeeID).Any())
                        return Json(new { success = false, message = "Önceden Seçilen Kursa Aynı Kayıt Mevcut." });

                }
                    foreach (var registeredCourse in classAttendeeViewModel.RegisteredCourses)
                {
                    Registrations registration = new Registrations
                    {
                        ClassID = _context.Classes.Where(i => i.Schedule == registeredCourse).Select(i => i.ClassID).FirstOrDefault(),
                        AttendeeID = classAttendeeViewModel.AttendeeID,
                        RegistrationTime = DateTime.Now,
                    };
                    try
                    {
                        _context.Registrations.Add(registration);
                        _context.SaveChanges();
                    }
                    catch (Exception)
                    {

                        return Json(new { success = false });
                    }

                }

                return Json(new { success = true, message = "Katılımcı Başarıyla Kaydedildi." });


            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Katılımcı Kaydedilirken Hata İle Karşılaşıldı." });
            }
        }

        public ActionResult OdemeTakip()
        {
            Cls_Panel panel = new Cls_Panel("Yasemin", "Kirpikli", "Yönetim");

            return View(panel);
        }

        public ActionResult InsertTeacherForClasses() 
        {
            var locationsToWork = _context.Locations
                            .Where(item => item.LocationTypeID == 1 && item.Open_ == true).Select(item => item.LocationName).Distinct().ToList();
            var existingTeachers = _context.Instructors
                .Select(item => string.Join(" ",item.FirstName, item.SecondName,item.LastName).Trim())
                .Distinct()
                .ToList();
            var classesToOffer = _context.ClassesMaster.Select(n=>n.ClassName).Distinct().ToList();
            var eventsToOffer = _context.Events.Select(n=>n.EventName).Distinct().ToList();
            var challangesToOffer = _context.ChallengesMaster.Select(n=> n.ChallengeName).Distinct().ToList();
             
            var insertTeacher = new InsertTeacherViewModel
            {
                LocationsToWork = locationsToWork,
                ExistingTeachers = existingTeachers,
                ClassesToOffer = classesToOffer,
                EventsToOffer = eventsToOffer,
                ChallengesToOffer = challangesToOffer,

            };

            return View(insertTeacher);
        }
        [HttpPost]
        public ActionResult InsertTeacherForClasses(InsertTeacherViewModel teacher) 
        {
            Instructors instructor = new();

            if (string.IsNullOrEmpty(teacher.ParentTeacherName))
                instructor.ParentInstructorID = "0";
            else
            {
                string[] parentTeacherNames = teacher.ParentTeacherName.Split(',');
                counter = 1;
                    
                foreach (string name in parentTeacherNames)
                {
                    if(parentTeacherNames.Count() == 1 ||
                       counter == parentTeacherNames.Count()) 
                        instructor.ParentInstructorID += _context.Instructors.Where(n=>string.Join(" ",n.FirstName,n.SecondName,n.LastName).Trim() == name).Select(i=>i.InstructorID.ToString());
                    else 
                        instructor.ParentInstructorID += _context.Instructors.Where(n => string.Join(" ", n.FirstName, n.SecondName, n.LastName).Trim() == name).Select(i => i.InstructorID.ToString() + ",");

                    counter++;
                }
            }

            instructor.FirstName = teacher.TeacherToRecord.FirstName;

            if (!string.IsNullOrEmpty(instructor.SecondName))
                instructor.SecondName = teacher.TeacherToRecord.SecondName;
            else
                instructor.SecondName = "";

            instructor.LastName = teacher.TeacherToRecord.LastName;
            instructor.Email = teacher.TeacherToRecord.Email;
            instructor.Phone = teacher.TeacherToRecord.Phone;
            instructor.Title = teacher.TeacherToRecord.Title;

            if (string.IsNullOrEmpty(teacher.StudiosWorkFor))
                instructor.StudiosWorkFor = "0";
            else
            {
                string[] StudiosWorkFor = teacher.StudiosWorkFor.Split(',');
                counter = 1;

                    foreach (string name in StudiosWorkFor)
                    {
                        if (StudiosWorkFor.Count() == 1 ||
                           counter == StudiosWorkFor.Count())
                            instructor.StudiosWorkFor += _context.Locations.Where(n => n.LocationName == name).Select(i => i.LocationID.ToString()).FirstOrDefault();
                        else
                            instructor.StudiosWorkFor += _context.Locations.Where(n => n.LocationName == name).Select(i => i.LocationID.ToString()+",").FirstOrDefault();

                            counter++;
                    }
            }

            instructor.IsParent = teacher.TeacherToRecord.IsParent;
            instructor.IsStudioOwner = teacher.TeacherToRecord.IsStudioOwner;

            if (string.IsNullOrEmpty(teacher.StudiosOwned))
                instructor.StudiosOwned = "0";
            else
            {
                string[] StudiosOwned = teacher.StudiosOwned.Split(',');
                counter = 1;

                foreach (string name in StudiosOwned)
                {
                    if (StudiosOwned.Count() == 1 ||
                       counter == StudiosOwned.Count())
                        instructor.StudiosOwned += _context.Locations.Where(n => n.LocationName == name).Select(i => i.LocationID.ToString()).FirstOrDefault();
                    else
                        instructor.StudiosOwned += _context.Locations.Where(n => n.LocationName == name).Select(i => i.LocationID.ToString() + ",").FirstOrDefault();

                    counter++;
                }
            }

            instructor.Bio = teacher.TeacherToRecord.Bio;
            instructor.Slogan = teacher.TeacherToRecord.Slogan;
            instructor.SloganSub = teacher.TeacherToRecord.SloganSub;
            instructor.PhilosophyAndEducationStyleMainSentence = teacher.TeacherToRecord.PhilosophyAndEducationStyleMainSentence;
            instructor.PhilosophyAndEducationStylePhilosophy = teacher.TeacherToRecord.PhilosophyAndEducationStylePhilosophy;
            instructor.PhilosophyAndEducationStyleEducationStyle = teacher.TeacherToRecord.PhilosophyAndEducationStyleEducationStyle;
            instructor.PhilosophyAndEducationStyleFocus = teacher.TeacherToRecord.PhilosophyAndEducationStyleFocus;

            if (teacher.TeacherToRecord.FeaturedCourse != null)
                instructor.FeaturedCourse = teacher.TeacherToRecord.FeaturedCourse;
            else
                instructor.FeaturedCourse = 0;


            if (!string.IsNullOrEmpty(teacher.ClassesGiven))
            { 
                string[] classesGiven = teacher.ClassesGiven.Split(',');
                counter = 1;

                foreach (string name in classesGiven)
                {
                    if (counter == 1)
                        instructor.CoursesOffers1 = name;
                    if (counter == 2)
                        instructor.CoursesOffers2 = name;
                    if (counter == 3)
                        instructor.CoursesOffers3 = name;
                    if (counter == 4)
                        instructor.CoursesOffers4 = name;
                    if (counter == 5)
                        instructor.CoursesOffers5 = name;
                    if(counter > 5)
                        return Json(new { success = false, message = "5'ten Fazla Kurs Eklenemez." });
                    
                    counter++;
                }
            }

            if (!string.IsNullOrEmpty(teacher.EventsGiven))
            {
                string[] eventsGiven = teacher.EventsGiven.Split(',');
                counter = 1;

                foreach (string name in eventsGiven)
                {
                    if (counter == 1)
                        instructor.EventsOffers1 = name;
                    if (counter == 2)
                        instructor.EventsOffers2 = name;
                    if (counter == 3)
                        instructor.EventsOffers3 = name;
                    if (counter == 4)
                        instructor.EventsOffers4 = name;
                    if (counter == 5)
                        instructor.EventsOffers5 = name;
                    if (counter > 5)
                        return Json(new { success = false, message = "5'ten Fazla Etkinlik Eklenemez." });

                    counter++;
                }
            }

            if (!string.IsNullOrEmpty(teacher.ChallengeGiven))
            {
                string[] classesGiven = teacher.ClassesGiven.Split(',');
                counter = 1;

                foreach (string name in classesGiven)
                {
                    if (counter == 1)
                        instructor.ChallengesOffers1 = name;
                    if (counter == 2)
                        instructor.ChallengesOffers2 = name;
                    if (counter == 3)
                        instructor.ChallengesOffers3 = name;
                    if (counter == 4)
                        instructor.ChallengesOffers4 = name;
                    if (counter == 5)
                        instructor.ChallengesOffers5 = name;
                    if (counter > 5)
                        return Json(new { success = false, message = "5'ten Fazla Meydan Okuma Eklenemez." });

                    counter++;
                }
            }

            instructor.Active_ = true;
            instructor.CreatedAt = DateTime.Now;
            instructor.CreatedBy = 0;
            instructor.EditedBy = 0;
            instructor.EditedAt = DateTime.Now;

            try
            {
                _context.Instructors.Add(instructor);
                _context.SaveChanges();
            }
            catch 
            {
                return Json(new { success = false, message = "Eğitmen Kaydedilirken Hata İle Karşılaşıldı." });
            }

            var locationsToWork = _context.Locations
                            .Where(item => item.LocationTypeID == 1 && item.Open_ == true).Select(item => item.LocationName).Distinct().ToList();
            var existingTeachers = _context.Instructors
                .Select(item => string.Join(" ", item.FirstName, item.SecondName, item.LastName).Trim())
                .Distinct()
                .ToList();
            var classesToOffer = _context.ClassesMaster.Select(n => n.ClassName).Distinct().ToList();
            var eventsToOffer = _context.Events.Select(n => n.EventName).Distinct().ToList();
            var challangesToOffer = _context.ChallengesMaster.Select(n => n.ChallengeName).Distinct().ToList();

            var insertTeacher = new InsertTeacherViewModel
            {
                LocationsToWork = locationsToWork,
                ExistingTeachers = existingTeachers,
                ClassesToOffer = classesToOffer,
                EventsToOffer = eventsToOffer,
                ChallengesToOffer = challangesToOffer,

            };

            return View(insertTeacher);
        }
        
        public ActionResult InsertTeacherPicturesandLinks()
        {
            InsertTeacherPicturesAndLinksViewModel model = new();
            model.InstructorInfo = _context.Instructors
                .Select(i => new
                {
                    Id = i.InstructorID,
                    FullName = string.Join(" ", i.FirstName, i.SecondName, i.LastName).Trim()
                })
                .ToDictionary(i => i.Id, i => i.FullName);
            return View(model);
        }
        public ActionResult InsertCoursesPicturesandLinks()
        {
            InsertCoursesPicturesAndLinksViewModel model = new();
            model.CourseInfo = _context.ClassesMaster
                .Select(c => new
                {
                    Id = c.ClassMasterID,
                    Name = c.ClassName
                })
                .ToDictionary(c => c.Id, c => c.Name);
            return View(model);
        }

        [HttpPost]
        [Route("/Panel/InsertTeacherPicturesandLinks/UpdatePhotos")]
        public IActionResult InsertTeacherPicturesandLinks(string teacherName,int teacherID,string photoName, IFormFile file )
        {
            teacherID = Convert.ToInt32(teacherID);

            if (string.IsNullOrEmpty(teacherName))
            {
                return Json(new { success = false, message = "Eğitmen seçiniz!" });
            }

            if (file != null && file.Length > 0)
            {
                string teacherNamePath = teacherName.Replace(" ", "");
                string photoNamePath = photoName.Replace(" ", "");

                string uploadDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "images/Teachers", teacherNamePath);

                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                string originalFileName = Path.GetFileName(file.FileName);
                string fileType = Path.GetExtension(originalFileName);
                string newFileName = $"{teacherNamePath}_{photoNamePath}{fileType}";
                string filePath = Path.Combine(uploadDirectory, newFileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                if (!System.IO.File.Exists(filePath))
                    return Json(new { success = false, message = "Dosya Yüklenemedi." });

                Instructors instructorToUpdate = _context.Instructors.Where(i => i.InstructorID == teacherID).FirstOrDefault();

                if (instructorToUpdate == null)
                    return Json(new { success = false, message = "Öğretmenin Sistemde Kaydı Bulunamadı." });

                switch (photoName)
                {
                    case "Panel Fotosu":
                        {
                            instructorToUpdate.PhotoPathForPanel = newFileName;
                            break;
                        }
                    case "Takım Sayfası Fotosu":
                        {
                            instructorToUpdate.PhotoPathForTeam = newFileName;
                            break;
                        }
                    case "Hakkımda Sayfası Ana Foto":
                        {
                            instructorToUpdate.PhotoPathForAboutMe = newFileName;
                            break;
                        }
                    case "Hakkımda Sayfası 1.Resim":
                        {
                            instructorToUpdate.PhotoPathForAboutMeSub1 = newFileName;
                            break;
                        }
                    case "Hakkımda Sayfası 2.Resim":
                        {
                            instructorToUpdate.PhotoPathForAboutMeSub2 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 1.Resim":
                        {
                            instructorToUpdate.GalleryPhotoPath1 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 2.Resim":
                        {
                            instructorToUpdate.GalleryPhotoPath2 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 3.Resim":
                        {
                            instructorToUpdate.GalleryPhotoPath3 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 4.Resim":
                        {
                            instructorToUpdate.GalleryPhotoPath4 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 5.Resim":
                        {
                            instructorToUpdate.GalleryPhotoPath5 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 6.Resim":
                        {
                            instructorToUpdate.GalleryPhotoPath6 = newFileName;
                            break;
                        }
                    case "Bahsedildi İkon 1.Resim":
                        {
                            instructorToUpdate.FeaturedInPhotoPath1 = newFileName;
                            break;
                        }
                    case "Bahsedildi İkon 2.Resim":
                        {
                            instructorToUpdate.FeaturedInPhotoPath2 = newFileName;
                            break;
                        }
                    case "Bahsedildi İkon 3.Resim":
                        {
                            instructorToUpdate.FeaturedInPhotoPath3 = newFileName;
                            break;
                        }
                    case "Bahsedildi İkon 4.Resim":
                        {
                            instructorToUpdate.FeaturedInPhotoPath4 = newFileName;
                            break;
                        }
                    case "Bahsedildi İkon 5.Resim":
                        {
                            instructorToUpdate.FeaturedInPhotoPath5 = newFileName;
                            break;
                        }

                }

                instructorToUpdate.EditedBy = 0;
                instructorToUpdate.EditedAt = DateTime.Now;

                try
                {
                    _context.Instructors.Update(instructorToUpdate);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Kayıt Güncellendi." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message.ToString() });
                }

            }
            else
            {
                return Json(new { success = false, message = "Dosya Seçilmedi." });
            }

        }
        [HttpPost]
        [Route("/Panel/InsertTeacherPicturesandLinks/UpdateLinks")]
        public IActionResult InsertTeacherPicturesandLinks(string teacherName, int teacherID, string linkType, string linkAddress)
        {
            if (string.IsNullOrEmpty(teacherName))
            {
                return Json(new { success = false, message = "Eğitmen Seçiniz!" });
            }
            if (string.IsNullOrEmpty(linkType))
            {
                return Json(new { success = false, message = "Link Tipi Seçiniz!" });
            }
            if (string.IsNullOrEmpty(linkAddress))
            {
                return Json(new { success = false, message = "Adres Giriniz!" });
            }

            try
            {
                Instructors instructorToUpdate = _context.Instructors.Where(i => i.InstructorID == teacherID).FirstOrDefault();
                
                if (instructorToUpdate == null)
                    return Json(new { success = false, message = "Öğretmenin Sistemde Kaydı Bulunamadı." });

                switch (linkType)
                {
                    case "Instagram":
                    {
                        instructorToUpdate.InstagramLink = linkAddress;
                        break;
                    }
                    case "Facebook":
                    {
                            instructorToUpdate.FacebookLink = linkAddress;
                        break;
                    }
                    case "Twitter":
                    {
                            instructorToUpdate.TwitterLink = linkAddress;
                           
                        break;
                    }
                    case "Bahsedildi1":
                    {
                            instructorToUpdate.FeaturedInLink1 = linkAddress;
                           
                        break;
                    }
                    case "Bahsedildi2":
                    {
                            instructorToUpdate.FeaturedInLink2 = linkAddress;
                          
                        break;
                    }
                    case "Bahsedildi3":
                    {
                            instructorToUpdate.FeaturedInLink3 = linkAddress;
                           
                        break;
                    }
                    case "Bahsedildi4":
                    {
                            instructorToUpdate.FeaturedInLink4 = linkAddress;
                            
                        break;
                    }
                    case "Bahsedildi5":
                    {
                            instructorToUpdate.FeaturedInLink5 = linkAddress;
                            
                        break;
                    }
                }

                instructorToUpdate.EditedBy = 0;
                instructorToUpdate.EditedAt = DateTime.Now;

                try
                {
                    _context.Instructors.Update(instructorToUpdate);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Kayıt Güncellendi." });
                }
                catch (Exception ex) 
                {
                    return Json(new { success = false, message = ex.Message.ToString() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }
        
        [HttpPost]
        [Route("/Panel/InsertCoursePicturesandLinks/UpdatePhotos")]
        public IActionResult InsertCoursePicturesandLinks(string className,int classID,string photoName, IFormFile file )
        {
            classID = Convert.ToInt32(classID);

            if (string.IsNullOrEmpty(className))
            {
                return Json(new { success = false, message = "Sınıf seçiniz!" });
            }

            if (file != null && file.Length > 0)
            {
                string classNamePath = className.Replace(" ", "") + classID;
                string photoNamePath = photoName.Replace(" ", "");

                string uploadDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "images/Classes", classNamePath);

                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                string originalFileName = Path.GetFileName(file.FileName);
                string fileType = Path.GetExtension(originalFileName);
                string newFileName = $"{classNamePath}_{photoNamePath}{fileType}";
                string filePath = Path.Combine(uploadDirectory, newFileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                if (!System.IO.File.Exists(filePath))
                    return Json(new { success = false, message = "Dosya Yüklenemedi." });

                ClassesMaster classToUpdate = _context.ClassesMaster.Where(i => i.ClassMasterID == classID).FirstOrDefault();

                if (classToUpdate == null)
                    return Json(new { success = false, message = "Sınıfın Sistemde Kaydı Bulunamadı." });

                switch (photoName)
                {
                    case "Kurs Fotosu":
                        {
                            classToUpdate.Photopath1 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 1.Resim":
                        {
                            classToUpdate.Photopath2 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 2.Resim":
                        {
                            classToUpdate.Photopath3 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 3.Resim":
                        {
                            classToUpdate.Photopath4 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 4.Resim":
                        {
                            classToUpdate.Photopath5 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 5.Resim":
                        {
                            classToUpdate.Photopath6 = newFileName;
                            break;
                        }
                    case "Galeri Fotosu 6.Resim":
                        {
                            classToUpdate.Photopath7 = newFileName;
                            break;
                        }

                }

                classToUpdate.EditedBy = 0;
                classToUpdate.EditedAt = DateTime.Now;

                try
                {
                    _context.ClassesMaster.Update(classToUpdate);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Kayıt Güncellendi." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message.ToString() });
                }

            }
            else
            {
                return Json(new { success = false, message = "Dosya Seçilmedi." });
            }

        }
        [HttpPost]
        [Route("/Panel/InsertCoursePicturesandLinks/UpdateLinks")]
        public IActionResult InsertCoursePicturesandLinks(string className, int classID, string linkAddress)
        {
            if (string.IsNullOrEmpty(className))
            {
                return Json(new { success = false, message = "Sınıf Seçiniz!" });
            }
            if (string.IsNullOrEmpty(linkAddress))
            {
                return Json(new { success = false, message = "Adres Giriniz!" });
            }

            try
            {
                ClassesMaster classToUpdate = _context.ClassesMaster.Where(i => i.ClassMasterID == classID).FirstOrDefault();
                
                if (classToUpdate == null)
                    return Json(new { success = false, message = "Sınıfın Sistemde Kaydı Bulunamadı." });

                classToUpdate.VideoPath = linkAddress;

                classToUpdate.EditedBy = 0;
                classToUpdate.EditedAt = DateTime.Now;

                try
                {
                    _context.ClassesMaster.Update(classToUpdate);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Kayıt Güncellendi." });
                }
                catch (Exception ex) 
                {
                    return Json(new { success = false, message = ex.Message.ToString() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        public IActionResult InsertStudio()
        { 
            try 
	        {
                return View();
	        }
	        catch
	        {
                return View("NotFound");
	        }
        }
        public ActionResult NotFound()
        { 
            return View(); 
        }
    }
}
