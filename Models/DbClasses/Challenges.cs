using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Models.DbClasses
{
    public class Challenges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChallengeID { get; set; }

        [ForeignKey("ChallengeMaster")]
        public int ChallengeMasterID { get; set; }

        [Required]
        public string ChallengeName { get; set; }

        [Required]
        public DateTime Schedule { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        public decimal Price { get; set; }

    }
}
