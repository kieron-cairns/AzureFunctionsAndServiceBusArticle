using System.ComponentModel.DataAnnotations;

namespace DatabaseSaverWebAPI.Models
{
    public class FormSubmission
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Message { get; set; }

        [Required]
        public DateTime Submitted { get; set; }

        public bool IsContactFormSubmit { get; set; }

        public bool IsNewsLetterSubmit { get; set; }
    }
}
