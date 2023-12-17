using System.ComponentModel.DataAnnotations;

namespace CodeHub.Models
{
    public class UserDetails
    {
        [Key]
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Technology { get; set; }
        public int TotalQuestionRaise { get; set; }
        public int TotalAnswer { get; set; }
        public int TotalVoting { get; set; }
        public string? GitHub { get; set; }
        public string? LinkedIn { get; set; }
        public string? Email { get; set; }
        public string? About { get; set; }

    }
}
