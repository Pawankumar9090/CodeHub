using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CodeHub.Models
{
    public class Questions
    {
        [Key] 
        public int QId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [AllowNull]
        public string? UserName { get; set; }
        [Required]
        public string? Question { get; set; }
        [AllowNull]
        public string? QDescriptio { get; set; }
        [AllowNull]
        public string? QuestionTage { get; set; }
        [AllowNull]
        public string? AnsOrNot { get; set; }
        [AllowNull]
        public int TotalAnswer { get; set; }
        [AllowNull]
        public DateTime Date { get; set; }

    }
}
