using System.ComponentModel.DataAnnotations;

namespace CodeHub.Models
{
    public class Voting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AnsId { get; set; }
        [Required]
        public string? UserId { get; set;}
        [Required]
        public int Vote { get; set; } 
    }
}
