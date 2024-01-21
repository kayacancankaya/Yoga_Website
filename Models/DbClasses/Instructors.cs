using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Models.DbClasses
{
    public class Instructors
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstructorID { get; set; }
        [Required]
        public string ParentInstructorID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? SecondName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty.ToString();

        [Required]
        public string Bio { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Slogan { get; set; } = string.Empty;
        [Required]
        [MaxLength(500)]
        public string SloganSub { get; set; } = string.Empty;
        [Required]
        public string PhilosophyAndEducationStyleMainSentence { get; set; } = string.Empty;
        [Required]
        public string PhilosophyAndEducationStylePhilosophy { get; set; } = string.Empty;
        [Required]
        public string PhilosophyAndEducationStyleEducationStyle { get; set; } = string.Empty;
        [Required]
        public string PhilosophyAndEducationStyleFocus { get; set; } = string.Empty;

        public bool IsParent { get; set; } = false;

        public bool IsStudioOwner { get; set; } = false;
        public string StudiosOwned { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string StudiosWorkFor { get; set; } = string.Empty;
        public Int16? FeaturedCourse { get; set; }

        [MaxLength(100)]
        public string? CoursesOffers1 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? CoursesOffers2 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? CoursesOffers3 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? CoursesOffers4 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? CoursesOffers5 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? EventsOffers1 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? EventsOffers2 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? EventsOffers3 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? EventsOffers4 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? EventsOffers5 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? ChallengesOffers1 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? ChallengesOffers2 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? ChallengesOffers3 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? ChallengesOffers4 { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? ChallengesOffers5 { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public Int16 CreatedBy { get; set; } = 0;
        [Required]
        public DateTime EditedAt { get; set; } = DateTime.Now;
        [Required]
        public Int16 EditedBy { get; set; } = 0;
       
        public string PhotoPathForTeam { get; set; } = string.Empty;

        public string PhotoPathForPanel { get; set; } = string.Empty;

        public string PhotoPathForAboutMe { get; set; } = string.Empty;

        public string PhotoPathForAboutMeSub1 { get; set; } = string.Empty;

        public string PhotoPathForAboutMeSub2 { get; set; } = string.Empty;
        public string InstagramLink { get; set; } = string.Empty;
        public string FacebookLink { get; set; } = string.Empty;
        public string TwitterLink { get; set; } = string.Empty;
        public string PinterestLink { get; set; } = string.Empty;
        public string FeaturedInLink1 { get; set; } = string.Empty;
        public string FeaturedInLink2 { get; set; } = string.Empty;
        public string FeaturedInLink3 { get; set; } = string.Empty;
        public string FeaturedInLink4 { get; set; } = string.Empty;
        public string FeaturedInLink5 { get; set; } = string.Empty;
        public string FeaturedInPhotoPath1 { get; set; } = string.Empty;
        public string FeaturedInPhotoPath2 { get; set; } = string.Empty;
        public string FeaturedInPhotoPath3 { get; set; } = string.Empty;
        public string FeaturedInPhotoPath4 { get; set; } = string.Empty;
        public string FeaturedInPhotoPath5 { get; set; } = string.Empty;

        public string GalleryPhotoPath1 { get; set; } = string.Empty;
        public string GalleryPhotoPath2 { get; set; } = string.Empty;
        public string GalleryPhotoPath3 { get; set; } = string.Empty;
        public string GalleryPhotoPath4 { get; set; } = string.Empty;
        public string GalleryPhotoPath5 { get; set; } = string.Empty;
        public string GalleryPhotoPath6 { get; set; } = string.Empty;

        public bool Active_ { get; set; } = true;
        public ICollection<Classes> Classes { get; set; }

    }
}



