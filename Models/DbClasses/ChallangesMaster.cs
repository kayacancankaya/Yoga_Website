using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Models.DbClasses
{
    public class ChallangesMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChallengeMasterID { get; set; }

        [Required]
        public string ChallengeName { get; set; }

        [Required]
        public string ChallengeTitle { get; set; }

        [Required]
        public int InstructorID { get; set; }

        [Required]
        public int LocationID { get; set; }

        [Required]
        public int MaxCapacity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public string Level { get; set; }

        public string Related { get; set; }

        public int Popularity { get; set; } = 0;

        public string Description { get; set; }

        public string VideoPath { get; set; }

        public string PhotoPath1 { get; set; }

        public string PhotoPath2 { get; set; }

        public string PhotoPath3 { get; set; }

        public string PhotoPath4 { get; set; }

        public string PhotoPath5 { get; set; }

        public string PhotoPath6 { get; set; }

        public string PhotoPath7 { get; set; }

        public string PhotoPath8 { get; set; }

        public string PhotoPath9 { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public short CreatedBy { get; set; } = 0;

        public DateTime EditedAt { get; set; } = DateTime.Now;

        public short EditedBy { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}



