using CodeHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeHub.Data
{
    public class MyDbContext:IdentityDbContext<UserAccounts>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options) { }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Voting> Voting { get; set; }

    }
}
