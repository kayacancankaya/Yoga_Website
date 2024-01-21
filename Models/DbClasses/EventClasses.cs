using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Models.DbClasses
{
    public class EventClasses
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventClassID { get; set; }

        public int EventID { get; set; }
        public int ClassID { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClassName { get; set; }

        // You can add navigation properties if needed
    }
}

