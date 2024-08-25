using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class PersonalInfo
    {
        [Key]
        public int Id { get; set; } // Primary key

        [Column(TypeName = "text")]
        public string Identifier { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }
    }
}
