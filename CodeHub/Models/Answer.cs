using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CodeHub.Models
{
    public class Answer
    {
        [Key] public int AnsId { get; set; }
        [Required] public string? UserId { get; set; }
        [AllowNull]
        public string? UserName { get; set; }
        [Required]
        public string? Answers { get; set; }
        [AllowNull]
        public string? ADiscription { get; set; }
        [Required]
        public int QId { get; set; }
        [Required]
        public string? AnsTage { get; set; }
        [AllowNull]
        public int Vote { get; set; }
        [AllowNull]
        public DateTime Date { get; set; }
    }
}
