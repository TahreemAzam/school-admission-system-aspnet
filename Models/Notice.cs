using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebsite1.Models
{
    public class Notice
    {
        [Key]
        public int Id { get; set; }

        [Required] public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
