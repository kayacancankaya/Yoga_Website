using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Pan.Models.DbClasses;
using System.Globalization;

namespace Pan.Models.HomeViewModels
{
    public class ScheduleViewModel
    {
        public string SelectedDayForCalendar { get; set; }

        public int WeekNumberOfSelectedDay
        {
            get
            {
                if (DateTime.TryParseExact(SelectedDayForCalendar, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    CultureInfo ci = CultureInfo.CurrentCulture;
                    Calendar calendar = ci.Calendar;
                    int weekNumber = calendar.GetWeekOfYear(selectedDate, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
                    return weekNumber;
                }

                // Return 0 or handle the case where parsing fails
                return 0;
            }
        }
        public DateTime FirstDayOfTheWeek
        {
            get
            {
                if (DateTime.TryParseExact(SelectedDayForCalendar, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    // Find the day of the week for the selected date
                    DayOfWeek dayOfWeek = selectedDate.DayOfWeek;

                    // Calculate the difference between the selected day and the first day of the week
                    int daysUntilFirstDay = (int)dayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

                    // Adjust the selected date to get the first day of the week
                    DateTime firstDayOfWeek = selectedDate.AddDays(-daysUntilFirstDay);

                    return firstDayOfWeek;
                }

                // Return DateTime.MinValue or handle the case where parsing fails
                return DateTime.MinValue;
            }

           
        }

        public DateTime TuesdayOfTheWeek
        {
            get
            {
                if (DateTime.TryParseExact(SelectedDayForCalendar, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    // Calculate the difference between the selected day and Tuesday
                    int daysUntilTuesday = (int)DayOfWeek.Tuesday - (int)selectedDate.DayOfWeek;

                    // Adjust the selected date to get the Tuesday of the week
                    DateTime tuesdayOfTheWeek = selectedDate.AddDays(daysUntilTuesday);

                    return tuesdayOfTheWeek;
                }

                // Return DateTime.MinValue or handle the case where parsing fails
                return DateTime.MinValue;
            }
        }

        public DateTime WednesdayOfTheWeek
        {
            get
            {
                if (DateTime.TryParseExact(SelectedDayForCalendar, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    // Calculate the difference between the selected day and Wednesday
                    int daysUntilWednesday = (int)DayOfWeek.Wednesday - (int)selectedDate.DayOfWeek;

                    // Adjust the selected date to get the Wednesday of the week
                    DateTime wednesdayOfTheWeek = selectedDate.AddDays(daysUntilWednesday);

                    return wednesdayOfTheWeek;
                }

                // Return DateTime.MinValue or handle the case where parsing fails
                return DateTime.MinValue;
            }
        }

        public DateTime ThursdayOfTheWeek
        {
            get
            {
                if (DateTime.TryParseExact(SelectedDayForCalendar, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    // Calculate the difference between the selected day and Thursday
                    int daysUntilThursday = (int)DayOfWeek.Thursday - (int)selectedDate.DayOfWeek;

                    // Adjust the selected date to get the Thursday of the week
                    DateTime thursdayOfTheWeek = selectedDate.AddDays(daysUntilThursday);

                    return thursdayOfTheWeek;
                }

                // Return DateTime.MinValue or handle the case where parsing fails
                return DateTime.MinValue;
            }
        }

        public DateTime FridayOfTheWeek
        {
            get
            {
                if (DateTime.TryParseExact(SelectedDayForCalendar, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    // Calculate the difference between the selected day and Friday
                    int daysUntilFriday = (int)DayOfWeek.Friday - (int)selectedDate.DayOfWeek;

                    // Adjust the selected date to get the Friday of the week
                    DateTime fridayOfTheWeek = selectedDate.AddDays(daysUntilFriday);

                    return fridayOfTheWeek;
                }

                // Return DateTime.MinValue or handle the case where parsing fails
                return DateTime.MinValue;
            }
        }

        public DateTime SaturdayOfTheWeek
        {
            get
            {
                if (DateTime.TryParseExact(SelectedDayForCalendar, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    // Calculate the difference between the selected day and Saturday
                    int daysUntilSaturday = (int)DayOfWeek.Saturday - (int)selectedDate.DayOfWeek;

                    // Adjust the selected date to get the Saturday of the week
                    DateTime saturdayOfTheWeek = selectedDate.AddDays(daysUntilSaturday);

                    return saturdayOfTheWeek;
                }

                // Return DateTime.MinValue or handle the case where parsing fails
                return DateTime.MinValue;
            }
        }

        public DateTime LastDayOfTheWeek
        {
            get
            {
                if (DateTime.TryParseExact(SelectedDayForCalendar, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    int daysUntilSunday = 0;

                    if ((int)selectedDate.DayOfWeek == 1)
                        return selectedDate;
                    else
                    { daysUntilSunday = 7 - (int)selectedDate.DayOfWeek; }

                    // Adjust the selected date to get the Saturday of the week
                    DateTime lastDayOfTheWeek = selectedDate.AddDays(daysUntilSunday);

                    return lastDayOfTheWeek;
                }

                // Return DateTime.MinValue or handle the case where parsing fails
                return DateTime.MinValue;
            }
           
        }
        public DateTime NextWeekFirstDay
        {
            get
            {
                if (DateTime.TryParseExact(SelectedDayForCalendar, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
                {
                    // Find the day of the week for the selected date
                    DayOfWeek dayOfWeek = selectedDate.DayOfWeek;

                    // Calculate the difference between the selected day and the last day of the week
                    int daysUntilLastDay = 6 - (int)dayOfWeek;

                    // Adjust the selected date to get the last day of the week
                    DateTime nextWeekFirstDay = selectedDate.AddDays(daysUntilLastDay + 1);

                    return nextWeekFirstDay;
                }

                // Return DateTime.MinValue or handle the case where parsing fails
                return DateTime.MinValue;
            }
           
        }
        
        public List<Classes> classes { get; set; } = new ();
        public List<ClassesMaster> classesMaster { get; set; } = new();
        public List<Instructors> Instructors { get; set; } = new ();
        public List<ClassTypes> ClassTypes { get; set; } = new ();
        public List<Locations> Locations { get; set; } = new ();

        private readonly PanDbContext _context;

        public ScheduleViewModel(PanDbContext context)
        {
            _context = context;
        }

        public ScheduleViewModel UserEventModelUpdate(Dictionary<string, string> dropdownStates, 
                                                      Dictionary<string, bool> checkboxDayStates,
                                                      Dictionary<string, bool> checkboxDayTimeStates,
                                                      DateTime selectedDayForCalender)
        {
            try
            {

                ScheduleViewModel model = new(_context);
                model.SelectedDayForCalendar = selectedDayForCalender.ToString("dd.MM.yyyy");
                //get related weeks courses after the current time
                model.classes = _context.Classes.Where(d => d.Schedule > model.FirstDayOfTheWeek 
                                                        && d.Schedule < model.LastDayOfTheWeek
                                                        && d.Schedule > DateTime.Now).ToList();
                if(!dropdownStates.ContainsValue("Tüm Ders Tipleri") ||
                    !dropdownStates.ContainsValue("Tüm Şehirler") ||
                    !dropdownStates.ContainsValue("Tüm Stüdyolar") ||
                    !dropdownStates.ContainsValue("Tüm Eğitmenler"))
                    model.classes = CalculateScheduleRegardingDropdownStates(dropdownStates, model);

                if (checkboxDayStates.ContainsValue(false))
                    model.classes = CalculateScheduleRegardingCheckBoxDayStates(checkboxDayStates, model);
                
                if(checkboxDayTimeStates.ContainsValue(false))
                    model.classes = CalculateScheduleRegardingCheckBoxDayTimeStates(checkboxDayTimeStates, model);
                

                var instructorIDs = model.classes.Select(i => i.InstructorID).Distinct().ToList();
                var classTypes = model.classes.Select(c => c.ClassTypeID).Distinct().ToList();
                var places = model.classes.Select(l => l.LocationID).Distinct().ToList();
                var classMasterIDs = model.classes.Select(i=>i.ClassMasterID).Distinct().ToList();
                foreach (var item in instructorIDs)
                {
                    Instructors instructor = _context.Instructors.Where(i => i.InstructorID == item).FirstOrDefault();
                    model.Instructors.Add(instructor);
                }
                foreach (var item in classTypes)
                {
                    ClassTypes classType = _context.ClassTypes.Where(i => i.ClassTypeID == item).FirstOrDefault();
                    model.ClassTypes.Add(classType);
                }
                foreach (var item in places)
                {
                    Locations locations = _context.Locations.Where(l => l.LocationID == item).FirstOrDefault();
                    model.Locations.Add(locations);
                }
                foreach (var item in classMasterIDs)
                {
                    ClassesMaster classMaster = _context.ClassesMaster.Where(n => n.ClassMasterID == item).FirstOrDefault();
                    model.classesMaster.Add(classMaster);
                }

                return model;
            }
            catch
            {
                return null;
            }
        }

        public List<Classes> CalculateScheduleRegardingDropdownStates(Dictionary<string, string> dropdownStates, 
                                                                         ScheduleViewModel model)
        {
            try
            {
                foreach (var item in dropdownStates) 
                { 
                    if (item.Key == "ClassTypeDropDown" && item.Value != "Tüm Ders Tipleri" )
                    {
                        var classTypeID = _context.ClassTypes.Where(c=> c.ClassName == item.Value).Select(i=>i.ClassTypeID).FirstOrDefault();
                        model.classes =model.classes.Where(c => c.ClassTypeID == classTypeID).ToList();
                        
                    }
                    if (item.Key == "CityDropDown" && item.Value != "Tüm Şehirler" )
                    {
                        var locationIDs = _context.Locations.Where(l=> l.City == item.Value).Select(i=>i.LocationID).ToList();
                        model.classes = model.classes.Where(c => locationIDs.Contains(c.LocationID)).ToList();
                       
                    }
                    if (item.Key == "StudioDropDown" && item.Value != "Tüm Stüdyolar" )
                    {
                        var locationID = _context.Locations.Where(l=> l.LocationName == item.Value).Select(i=>i.LocationID).FirstOrDefault();
                        
                        model.classes = model.classes.Where(l => l.LocationID == locationID).ToList();
                     
                    }
                    if (item.Key == "TeacherDropDown" && item.Value != "Tüm Eğitmenler" )
                    {
                        var instructorID = _context.Instructors.Where(i=> i.FirstName + " " + i.LastName  == item.Value).Select(i=>i.InstructorID).FirstOrDefault();
                        
                        model.classes = model.classes.Where(l => l.LocationID == instructorID).ToList();
                     
                    }
                    
                }

                return model.classes;
            }
            catch 
            {
                return null;
            }
        }
        
        public List<Classes> CalculateScheduleRegardingCheckBoxDayStates(Dictionary<string, bool> checkboxDayStates, 
                                                                         ScheduleViewModel model)
        {
            try
            {
                List<Classes> classes = new ();
                foreach (var item in checkboxDayStates) 
                { 
                    if (item.Key == "Monday" && item.Value)
                    {
                       classes.AddRange(_context.Classes.Where(d => d.Schedule > model.FirstDayOfTheWeek
                                                        && d.Schedule < model.TuesdayOfTheWeek
                                                        && d.Schedule > DateTime.Now).ToList());
                        

                    }
                    if (item.Key == "Tuesday" && item.Value)
                    {
                        classes.AddRange(_context.Classes.Where(d => d.Schedule > model.TuesdayOfTheWeek
                                                        && d.Schedule < model.WednesdayOfTheWeek
                                                        && d.Schedule > DateTime.Now).ToList());
                    }
                    if (item.Key == "Wednesday" && item.Value)
                    {
                        classes.AddRange(_context.Classes.Where(d => d.Schedule > model.WednesdayOfTheWeek
                                                        && d.Schedule < model.ThursdayOfTheWeek
                                                        && d.Schedule > DateTime.Now).ToList());
                    }
                    if (item.Key == "Thursday" && item.Value)
                    {
                        classes.AddRange(_context.Classes.Where(d => d.Schedule > model.ThursdayOfTheWeek
                                                        && d.Schedule < model.FridayOfTheWeek
                                                        && d.Schedule > DateTime.Now).ToList());
                    }
                    if (item.Key == "Friday" && item.Value)
                    {
                        classes.AddRange(_context.Classes.Where(d => d.Schedule > model.FridayOfTheWeek
                                                        && d.Schedule < model.SaturdayOfTheWeek
                                                        && d.Schedule > DateTime.Now).ToList());
                    }
                    if (item.Key == "Saturday" && item.Value)
                    {
                        classes.AddRange(_context.Classes.Where(d => d.Schedule > model.SaturdayOfTheWeek
                                                        && d.Schedule < model.LastDayOfTheWeek
                                                        && d.Schedule > DateTime.Now).ToList());
                    }
                    if (item.Key == "Sunday" && item.Value)
                    {
                        classes.AddRange(_context.Classes.Where(d => d.Schedule > model.LastDayOfTheWeek
                                                        && d.Schedule < model.NextWeekFirstDay
                                                        && d.Schedule > DateTime.Now).ToList());
                    }
                }

                return classes;
            }
            catch 
            {
                return null;
            }
        }

        public List<Classes> CalculateScheduleRegardingCheckBoxDayTimeStates(Dictionary<string, bool> checkboxDayTimeStates,
                                                                             ScheduleViewModel model)
        {
            try
            {
                foreach (var item in checkboxDayTimeStates)
                {
                    if (item.Value)
                    {
                        if (item.Key == "Morning" && item.Value)
                        {
                            model.classes = model.classes
                                .Where(d => d.Schedule.TimeOfDay >= new TimeSpan(6, 0, 0) && d.Schedule.TimeOfDay < new TimeSpan(12, 0, 0))
                                .ToList();
                        }
                        else if (item.Key == "Noon" && item.Value)
                        {
                            model.classes = model.classes
                                .Where(d => d.Schedule.TimeOfDay >= new TimeSpan(12, 0, 0) && d.Schedule.TimeOfDay < new TimeSpan(18, 0, 0))
                                .ToList();
                        }
                        else if (item.Key == "Evening" && item.Value)
                        {
                            model.classes = model.classes
                                .Where(d => d.Schedule.TimeOfDay >= new TimeSpan(18, 0, 0) && d.Schedule.TimeOfDay < new TimeSpan(23, 59, 59))
                                .ToList();
                        }
                    }
                }

                return model.classes;
            }
            catch
            {
                return null;
            }
        }


    }
}
