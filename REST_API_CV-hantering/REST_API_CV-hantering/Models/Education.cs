using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_API_CV_hantering.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string School { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Degree { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FieldOfStudy { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; } // EndDate can be null if it's ongoing

        [ForeignKey("PersonId")]
        [Required]
        public int PersonId { get; set; }
    }
}
