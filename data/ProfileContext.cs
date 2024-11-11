using Microsoft.EntityFrameworkCore;
using ProfileService.Models;

namespace ProfileService.Data
{
    public class ProfileContext : DbContext
    {
    

        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; }

        
    }


}
