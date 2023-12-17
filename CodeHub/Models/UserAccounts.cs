using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CodeHub.Models
{
    public class UserAccounts:IdentityUser
    {
        [Required]
        public string? Name { get; set; }
        [AllowNull]
        public string? Technology { get; set; }

    }
}
