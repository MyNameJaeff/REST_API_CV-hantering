using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_API_CV_hantering.Models
{
    public class WorkExperience
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Company { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        [ForeignKey("PersonId"), Required]
        public int PersonId { get; set; }
    }
}
