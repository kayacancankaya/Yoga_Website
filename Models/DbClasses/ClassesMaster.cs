using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pan.Models.HomeViewModels;

namespace Pan.Models.DbClasses
{
    public class ClassesMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassMasterID { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClassName { get; set; } = string.Empty;
        [Required]
        [MaxLength(500)]
        public string ClassTitle { get; set; } = string.Empty;
        [Required]
        public int InstructorID { get; set; }
        [Required]
        public int ClassTypeID { get; set; }

        [Required]
        public int LocationID { get; set; }
        [Required]
        public int MaxCapacity { get; set; }

        public decimal TotalPrice { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Level { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Related { get; set; } = string.Empty;

        public int Popularity { get; set; }

        public string? VideoPath { get; set; }
        public string? Photopath1 { get; set; }
        public string? Photopath2 { get; set; }
        public string? Photopath3 { get; set; }
        public string? Photopath4 { get; set; }
        public string? Photopath5 { get; set; }
        public string? Photopath6 { get; set; }
        public string? Photopath7 { get; set; }
        public string? Photopath8 { get; set; }
        public string? Photopath9 { get; set; }
        public string? Description { get; set; } 

        public double DiscountRate1 { get; set; } = 5;
        public double DiscountRate2 { get; set; } = 10;
        public double DiscountRate3 { get; set; } = 15;
        public double DiscountRate4 { get; set; } = 25;
        public double DiscountRate5 { get; set; } = 50;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public int CreatedBy { get; set; } = 0;
        [Required]
        public DateTime EditedAt { get; set; } = DateTime.Now;
        [Required]
        public int EditedBy { get; set; } = 0;

        [ForeignKey("InstructorID")]
        public Instructors Instructor { get; set; }

        [ForeignKey("ClassTypeID")]
        public ClassTypes ClassType { get; set; }

        [ForeignKey("LocationID")]
        public Locations Locations { get; set; }

        private readonly PanDbContext _context;
        public ClassesMaster(PanDbContext context)
        {
            _context = context;
        }
        public ClassesMaster()
        {
         
        }

        public List<ClassesMaster> GetRelatedClasses(int classMasterID, int numberOfClassDemanding)
        {
            try
            {
                List<ClassesMaster> relatedClasses = new();

                if(!_context.ClassesMaster.Any(i=>i.ClassMasterID == classMasterID)) 
                {
                    return relatedClasses;
                }

                string? relatedCoursesString = _context.ClassesMaster
                                            .Where(i => i.ClassMasterID == classMasterID)
                                            .Select(r => r.Related)
                                            .FirstOrDefault();
                int counter = 0;

                string[] relatedCoursesArray;
                List<ClassesMaster> relatedCourses = new();
                List<ClassesMaster> coursesWithSameType = new();
                List<ClassesMaster> popularCourses = new();

                if (!string.IsNullOrEmpty(relatedCoursesString))
                {
                    relatedCoursesArray = relatedCoursesString.Split(',');
                    foreach (string item in relatedCoursesArray)
                    {
                        if (int.TryParse(item, out int result))
                        {
                            if (counter == numberOfClassDemanding)
                                return relatedCourses;

                            relatedCourses.Add(_context.ClassesMaster.Where(i => i.ClassMasterID == Convert.ToInt32(item)).FirstOrDefault());
                            counter++;
                        }
                    }
                }
                //if related courses are not numberOfClassDemanding find courses with same type if you can not
                //find numberOfClassDemanding courses again sort courses by popularity and populate the rest until list reaches numberOfClassDemanding.

                int classTypeID = _context.ClassesMaster
                                            .Where(i => i.ClassMasterID == classMasterID)
                                            .Select(i => i.ClassTypeID)
                                            .FirstOrDefault();

                coursesWithSameType = _context.ClassesMaster
                                                .Where(i => i.ClassTypeID == classTypeID)
                                                .ToList();

                foreach (ClassesMaster c in coursesWithSameType)
                {
                    if (counter == numberOfClassDemanding)
                        return relatedClasses;

                    relatedClasses.Add(c);

                    counter++;
                }

                popularCourses = _context.ClassesMaster
                                          .OrderByDescending(p => p.Popularity)
                                          .Take(numberOfClassDemanding - counter)
                                          .ToList();
                foreach(ClassesMaster p in popularCourses)
                {
                    if (counter == numberOfClassDemanding)
                        return relatedClasses;

                    relatedClasses.Add(p);

                    counter++;
                }
                return relatedClasses;

            }
            catch 
            {
                List<ClassesMaster> faultyClassList = new();
                return faultyClassList;
            }


        }
        public List<ClassesMaster> GetPopularClasses(int classMasterID, int numberOfClassDemanding)
        {
            try
            {
                List<ClassesMaster> popularClasses = new();

                if(!_context.ClassesMaster.Any(i=>i.ClassMasterID == classMasterID)) 
                {
                    return popularClasses;
                }
                var popularCourses = _context.ClassesMaster
                                          .OrderByDescending(p => p.Popularity)
                                          .Take(numberOfClassDemanding)
                                          .ToList();

                int counter = 0;
                foreach (ClassesMaster p in popularCourses)
                {
                    if (counter == numberOfClassDemanding)
                        return popularClasses;

                    popularClasses.Add(p);

                    counter++;
                }
                return popularClasses;

            }
            catch 
            {
                List<ClassesMaster> faultyClassList = new();
                return faultyClassList;
            }


        }
    }
}
