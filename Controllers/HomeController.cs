using Microsoft.AspNetCore.Mvc;
using Pan.Models;
using Pan.Models.DbClasses;
using Pan.Models.DbClasses.ViewModels;
using Pan.Models.HomeViewModels;
using System.Diagnostics;
using System.Globalization;

namespace Pan.Controllers
{
    public class HomeController : Controller
    {
        private readonly PanDbContext _context;
        private ScheduleViewModel _scheduleViewModel;
        bool result = false;
        public HomeController(PanDbContext context)
        {
            _context = context;
            _scheduleViewModel = new ScheduleViewModel(_context);
        }
        
        public IActionResult Index(string currentDate)
        {
            ContactUsViewModel contactUsViewModel = new();

            if (!string.IsNullOrEmpty(currentDate))
                _scheduleViewModel.SelectedDayForCalendar = currentDate;
            else
                _scheduleViewModel.SelectedDayForCalendar = DateTime.Now.ToString("dd.MM.yyyy");

            var classesExist = _context.Classes
                  .Any(d => d.Schedule > _scheduleViewModel.FirstDayOfTheWeek && d.Schedule < _scheduleViewModel.LastDayOfTheWeek);

            var indexViewModel = new IndexViewModel
            {
                schedule = _scheduleViewModel,
                contactUs = contactUsViewModel,
            };

            if (classesExist)
            {

                _scheduleViewModel.classes = _context.Classes
                    .Where(d => d.Schedule > _scheduleViewModel.FirstDayOfTheWeek && d.Schedule < _scheduleViewModel.LastDayOfTheWeek)
                    .ToList();

                var instructorIDs = _scheduleViewModel.classes.Select(i => i.InstructorID).Distinct().ToList();
                var classTypes = _scheduleViewModel.classes.Select(c => c.ClassTypeID).Distinct().ToList();
                var places = _scheduleViewModel.classes.Select(l => l.LocationID).Distinct().ToList();
                var masters = _scheduleViewModel.classes.Select(m => m.ClassMasterID).Distinct().ToList();
                foreach (var item in instructorIDs)
                {
                    Instructors instructor = _context.Instructors.Where(i => i.InstructorID == item).FirstOrDefault();
                    _scheduleViewModel.Instructors.Add(instructor);
                }
                foreach (var item in classTypes)
                {
                    ClassTypes classType = _context.ClassTypes.Where(i => i.ClassTypeID == item).FirstOrDefault();
                    _scheduleViewModel.ClassTypes.Add(classType);
                }
                foreach (var item in places)
                {
                    Locations locations = _context.Locations.Where(l => l.LocationID == item).FirstOrDefault();
                    _scheduleViewModel.Locations.Add(locations);
                }

                foreach (var item in masters)
                {
                    ClassesMaster master = _context.ClassesMaster.Where(l => l.ClassMasterID == item).FirstOrDefault();
                    _scheduleViewModel.classesMaster.Add(master);
                }

                indexViewModel.schedule = _scheduleViewModel;
              
                return View(indexViewModel);
            }
            
            else { return View(indexViewModel); }
        }
        public IActionResult Classes(int id)
        {

            ClassesMaster classesMaster = new(_context);
            Classes? model = _context.Classes.FirstOrDefault(i => i.ClassID == id);

            if (model != null)
            {
                string? classTypeName = _context.ClassTypes
                    .Where(i => i.ClassTypeID == model.ClassTypeID)
                    .Select(n => n.ClassName)
                    .FirstOrDefault();

                ViewBag.ClassTypeName = classTypeName;

                string? className = _context.ClassesMaster
                    .Where(i => i.ClassMasterID == model.ClassMasterID)
                    .Select(n => n.ClassName)
                    .FirstOrDefault();

                ViewBag.ClassName = className;

                int classMasterID = _context.ClassesMaster
                                                .Where(i => i.ClassMasterID == model.ClassMasterID)
                                                .Select(i => i.ClassMasterID)
                                                .FirstOrDefault();

                ViewBag.ClassMasterID = classMasterID;

                string? teacherName = _context.Instructors
                                     .Where(i => i.InstructorID == model.InstructorID).Select(n => n.FirstName + " " + n.LastName).FirstOrDefault();

                ViewBag.TeacherName = teacherName;

                ViewBag.TeacherID = model.InstructorID;

                var masterVars = _context.ClassesMaster
                                .Where(i => i.ClassMasterID == model.ClassMasterID)
                                .Select(m =>  new { m.Description , m.Level, });

                string level = string.Empty;
                string description = string.Empty;

                if (masterVars != null ) 
                { 
                    description = masterVars.Select(masterVars => masterVars.Description).FirstOrDefault();
                    level = masterVars.Select(m => m.Level).FirstOrDefault();
                }
                ViewBag.Description = description;
                ViewBag.Level = level;

                var relatedClasses = classesMaster.GetRelatedClasses(classMasterID, 5);
                Dictionary<int,string> relatedClassesTeacher = new();
                Dictionary<int,string> relatedClassesTypes = new();
                List<int> relatedClassIDs = new();
                
                if(relatedClasses.Count>0)
                { 
                    foreach (ClassesMaster item in relatedClasses) 
                    { 
                        if(!relatedClassesTeacher.ContainsKey(_context.Instructors.Where(i => i.InstructorID == item.InstructorID).Select(i => i.InstructorID).FirstOrDefault()))
                            relatedClassesTeacher.Add(_context.Instructors.Where(i=>i.InstructorID == item.InstructorID).Select(i=>i.InstructorID).FirstOrDefault(),
                                                      _context.Instructors.Where(i => i.InstructorID == item.InstructorID).Select(n => n.FirstName).FirstOrDefault());

                        if(!relatedClassesTypes.ContainsKey(_context.ClassTypes.Where(i => i.ClassTypeID == item.ClassTypeID).Select(i => i.ClassTypeID).FirstOrDefault()))
                            relatedClassesTypes.Add(_context.ClassTypes.Where(i => i.ClassTypeID == item.ClassTypeID).Select(i => i.ClassTypeID).FirstOrDefault(),
                                                _context.ClassTypes.Where(i =>i.ClassTypeID == item.ClassTypeID).Select(n=>n.ClassName).FirstOrDefault());

                        if(!relatedClassIDs.Contains(_context.Classes.Where(i => i.ClassMasterID == item.ClassMasterID).Select(i => i.ClassID).FirstOrDefault()))
                            relatedClassIDs.Add(_context.Classes.Where(i => i.ClassMasterID == item.ClassMasterID).Select(i => i.ClassID).FirstOrDefault());
                    }
                }


                ViewBag.RelatedClasses = relatedClasses;
                ViewBag.RelatedClassesTeacher = relatedClassesTeacher;
                ViewBag.RelatedClassesTypes = relatedClassesTypes;
                ViewBag.RelatedClassIDs = relatedClassIDs;

                var popularClasses = classesMaster.GetPopularClasses(classMasterID, 5);
                Dictionary<int,string> popularClassesTeacher = new();
                Dictionary<int,string> popularClassesTypes = new();
                List<int> popularClassIDs = new();
                
                if(popularClasses.Count>0)
                { 
                    foreach (ClassesMaster item in popularClasses) 
                    {
                        if (!popularClassesTeacher.ContainsKey(_context.Instructors.Where(i => i.InstructorID == item.InstructorID).Select(i => i.InstructorID).FirstOrDefault()))
                            popularClassesTeacher.Add(_context.Instructors.Where(i=>i.InstructorID == item.InstructorID).Select(i=>i.InstructorID).FirstOrDefault(),
                                                  _context.Instructors.Where(i => i.InstructorID == item.InstructorID).Select(n => n.FirstName).FirstOrDefault());

                        if (!popularClassesTypes.ContainsKey(_context.ClassTypes.Where(i => i.ClassTypeID == item.ClassTypeID).Select(i => i.ClassTypeID).FirstOrDefault()))
                            popularClassesTypes.Add(_context.ClassTypes.Where(i => i.ClassTypeID == item.ClassTypeID).Select(i => i.ClassTypeID).FirstOrDefault(),
                                                _context.ClassTypes.Where(i =>i.ClassTypeID == item.ClassTypeID).Select(n=>n.ClassName).FirstOrDefault());
                        
                        if (!popularClassIDs.Contains(_context.Classes.Where(i => i.ClassMasterID == item.ClassMasterID).Select(i => i.ClassID).FirstOrDefault()))
                            popularClassIDs.Add(_context.Classes.Where(i => i.ClassMasterID == item.ClassMasterID).Select(i => i.ClassID).FirstOrDefault());
                    }
                }

                ViewBag.PopularClasses = popularClasses;
                ViewBag.PopularClassesTeacher = popularClassesTeacher;
                ViewBag.PopularClassesTypes = popularClassesTypes;
                ViewBag.PopularClassIDs = popularClassIDs;
                return View(model);
            }

            return NotFound();
        }
        public IActionResult ClassesAll()
        {
            ClassListsViewModel? classList = new();

                classList.Classes = _context.Classes.Where(s => s.Schedule > DateTime.Now).ToList();

                foreach (Classes item in classList.Classes)
                {
                    if(!classList.ClassesMaster.Contains(_context.ClassesMaster.FirstOrDefault(i => i.ClassMasterID == item.ClassMasterID)))
                        classList.ClassesMaster.Add(_context.ClassesMaster.FirstOrDefault(i => i.ClassMasterID == item.ClassMasterID));

                    if(!classList.ClassTypes.Contains(_context.ClassTypes.FirstOrDefault(i => i.ClassTypeID == item.ClassTypeID)))
                        classList.ClassTypes.Add(_context.ClassTypes.FirstOrDefault(i => i.ClassTypeID == item.ClassTypeID));

                    if(!classList.Instructors.Contains(_context.Instructors.FirstOrDefault(i => i.InstructorID == item.InstructorID)))
                        classList.Instructors.Add(_context.Instructors.FirstOrDefault(i => i.InstructorID == item.InstructorID));

                    if(!classList.Locations.Contains(_context.Locations.FirstOrDefault(i => i.LocationID == item.LocationID)))
                        classList.Locations.Add(_context.Locations.FirstOrDefault(i => i.LocationID == item.LocationID));
                }
            
            return View(classList);
            
        }

        public IActionResult ClassTypes()
        {
            List<ClassTypes> classTypes = _context.ClassTypes.ToList();
            return View(classTypes);
        }

        [HttpPost]
        public IActionResult Previousupdateselecteddateindex(Dictionary<string, string> dropdownStates,
                                                             Dictionary<string, bool> checkboxDayStates,
                                                             Dictionary<string, bool> checkboxDayTimeStates,
                                                             string selectedDate)
        {
            DateTime selectedDateForCalendar = DateTime.Now;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                if (DateTime.TryParseExact(selectedDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDateForCalendarOld))
                {
                    selectedDateForCalendar = selectedDateForCalendarOld.AddDays(-7);
                }
            }

            _scheduleViewModel = _scheduleViewModel.UserEventModelUpdate(dropdownStates,checkboxDayStates, checkboxDayTimeStates, selectedDateForCalendar);

            //if(_scheduleViewModel == null)  
            //    return 
            return PartialView("schedule", _scheduleViewModel);
        }

        [HttpPost]
        public IActionResult Nextupdateselecteddateindex(Dictionary<string, string> dropdownStates,
                                                        Dictionary<string, bool> checkboxDayStates, 
                                                        Dictionary<string, bool> checkboxDayTimeStates, 
                                                        string selectedDate)
        {
            DateTime selectedDateForCalendar = DateTime.Now;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                if (DateTime.TryParseExact(selectedDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDateForCalendarOld))
                {
                    selectedDateForCalendar = selectedDateForCalendarOld.AddDays(7);
                }
            }

            _scheduleViewModel = _scheduleViewModel.UserEventModelUpdate(dropdownStates, checkboxDayStates, checkboxDayTimeStates, selectedDateForCalendar);

            //if(_scheduleViewModel == null)  
            //    return 
            return PartialView("schedule", _scheduleViewModel);
        }

        [HttpPost]
        public IActionResult Manualupdateselecteddateindex(Dictionary<string, string> dropdownStates, 
                                                           Dictionary<string, bool> checkboxDayStates,
                                                           Dictionary<string, bool> checkboxDayTimeStates,
                                                           string selectedDate)
        {
            
            DateTime selectedDateForCalendar = DateTime.Now;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                if (DateTime.TryParseExact(selectedDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDateForCalendarOld))
                {
                    selectedDateForCalendar = selectedDateForCalendarOld;
                }
            }

            _scheduleViewModel = _scheduleViewModel.UserEventModelUpdate(dropdownStates, checkboxDayStates, checkboxDayTimeStates, selectedDateForCalendar);

            //if(_scheduleViewModel == null)  
            //    return 
            return PartialView("schedule", _scheduleViewModel);
        }
        
        [HttpPost]
        public IActionResult Schedulefilterbuttoncliked(Dictionary<string, string> dropdownStates, 
                                                        Dictionary<string, bool> checkboxDayStates,
                                                        Dictionary<string, bool> checkboxDayTimeStates, 
                                                        string selectedDate)
        {
            
            DateTime selectedDateForCalendar = DateTime.Now;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                if (DateTime.TryParseExact(selectedDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDateForCalendarOld))
                {
                    selectedDateForCalendar = selectedDateForCalendarOld;
                }
            }

            _scheduleViewModel = _scheduleViewModel.UserEventModelUpdate(dropdownStates, checkboxDayStates, checkboxDayTimeStates, selectedDateForCalendar);

            //if(_scheduleViewModel == null)  
            //    return 
            return PartialView("schedule", _scheduleViewModel);
        }

        public IActionResult Newtoyoga()
        {
            return View();
        }

        public IActionResult Studyolarimiz()
        {
            return View();
        }
        public IActionResult Egitimlerimiz()
        {
            return View();
        }
        public IActionResult Etkinliklerimiz()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult InstructorPage(int id) 
        {
            try
            {

                Instructors? instructorPage = new();

                instructorPage = _context.Instructors.Where(i=> i.InstructorID == id).FirstOrDefault();

                if (instructorPage != null) { 

                return View(instructorPage);
                }

                return RedirectToAction("Not Found");

            }
            catch 
            {
                return RedirectToAction("Not Found");
            }
        }

        public IActionResult NotFound()
        { return View(); }

        public IActionResult ContactUs() 
        {
            try
            {
                ContactUsViewModel contactUs = new();
                LocationCardsViewModel location = new(_context);

                ContactUsPageViewModel contactUsPage = new();

                contactUsPage.contactUs = contactUs;
                contactUsPage.locationCards = location;

                return View(contactUsPage);
            }
            catch 
            {
                return RedirectToAction("NotFound");
            }
        }
    }
}