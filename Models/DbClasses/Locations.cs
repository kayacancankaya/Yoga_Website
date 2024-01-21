using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Models.DbClasses
{
    public class Locations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 LocationID { get; set; }

        [Required]
        [MaxLength(100)]
        public string LocationName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(50)]
        public string? WorkingHours { get; set; }
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        public string? GoogleMapsLink { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        [Required]
        public bool Open_ { get; set; }


        [Required]
        [Column("LocationTypeID")]
        public int LocationTypeID { get; set; }

        public string? AboutStudio { get; set; }

        public string? ReasonToChooseTitle1 { get; set; }
        public string? ReasonToChooseTitle2 { get; set; }
        public string? ReasonToChooseTitle3 { get; set; }
        public string? ReasonToChooseTitle4 { get; set; }
        public string? ReasonToChooseTitle5 { get; set; }
        public string? ReasonToChooseTitle6 { get; set; }
        
        public string? ReasonToChooseParagraph1 { get; set; }
        public string? ReasonToChooseParagraph2 { get; set; }
        public string? ReasonToChooseParagraph3 { get; set; }
        public string? ReasonToChooseParagraph4 { get; set; }
        public string? ReasonToChooseParagraph5 { get; set; }
        public string? ReasonToChooseParagraph6 { get; set; }

        public string? Mission { get; set; }
        public string? Vision { get; set; }
        public string? CoreValues { get; set; }

        public string? ClientName1 { get; set; }
        public string? ClientCity1 { get; set; }
        public string? ClientPhotoPath1 { get; set; }
        public string? ClientComment1 { get; set; }
        public string? ClientName2 { get; set; }
        public string? ClientCity2 { get; set; }
        public string? ClientPhotoPath2 { get; set; }
        public string? ClientComment2 { get; set; }
        public string? ClientName3 { get; set; }
        public string? ClientCity3 { get; set; }
        public string? ClientPhotoPath3 { get; set; }
        public string? ClientComment3 { get; set; }


        public string? StudioMainPhotoPath { get; set; }
        public string? StudioMainVideoPath { get; set; }

        public string? GalleryPhotoPath1 { get; set; }
        public string? GalleryPhotoPath2 { get; set; }
        public string? GalleryPhotoPath3 { get; set; }
        public string? GalleryPhotoPath4 { get; set; }
        public string? GalleryPhotoPath5 { get; set; }
        public string? GalleryPhotoPath6 { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public int CreatedBy { get; set; } = 0;
        [Required]
        public DateTime EditedAt { get; set; } = DateTime.Now;
        [Required]
        public int EditedBy { get; set; } = 0;

        public ICollection<Classes>? Classes { get; set; }
    }
}

