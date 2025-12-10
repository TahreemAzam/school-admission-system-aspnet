using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebsite1.Models
{
    public class Admission
    {
        [Key]
        public int Id { get; set; }

        [Required] public string ApplicantFullName { get; set; }
        [Required] public string ApplyingForClass { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        [Required] public string ParentGuardianName { get; set; }
        [Required] public string ParentContactNumber { get; set; }
        [EmailAddress] public string Email { get; set; }
        public string AlternateContact { get; set; }
        [Required] public string Address { get; set; }
        public string AdditionalNotes { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
