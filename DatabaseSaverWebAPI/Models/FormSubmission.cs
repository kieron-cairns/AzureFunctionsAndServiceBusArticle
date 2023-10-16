using System.ComponentModel.DataAnnotations;

namespace DatabaseSaverWebAPI.Models
{
    public class FormSubmission
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Message { get; set; }

        [Required]
        public DateTime Submitted { get; set; }

        public bool IsContactFormSubmit { get; set; }

        public bool IsNewsLetterSubmit { get; set; }
    }
}
