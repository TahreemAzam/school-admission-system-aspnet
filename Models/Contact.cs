using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebsite1.Models
{
    public class Contact
    {

        [Key]
        public int Id { get; set; }

        [Required] public string Name { get; set; }
        [EmailAddress] public string Email { get; set; }
        public string Phone { get; set; }
        [Required] public string Message { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
   
    }
}
