using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Models.DbClasses
{
    public class ClassTypes
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassTypeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClassName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Classes> Classes { get; set; }
       
    }
}


   

