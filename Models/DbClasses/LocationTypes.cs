using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Models.DbClasses
{
    public class LocationTypes
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationTypeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string LocationName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
