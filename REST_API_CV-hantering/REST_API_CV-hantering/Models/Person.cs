using System.ComponentModel.DataAnnotations;

namespace REST_API_CV_hantering.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(255)]
        public string ContactInfo { get; set; } = string.Empty;

        public List<Education> Educations { get; set; } = new();
        public List<WorkExperience> WorkExperiences { get; set; } = new();
    }
}
