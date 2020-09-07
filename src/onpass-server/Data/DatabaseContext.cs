using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using onpass_server.Models;

namespace onpass_server.Data
{
    /// <summary>
    /// App DBContext
    /// </summary>
    public class DatabaseContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Constructor for class
        /// </summary>
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        
        /// <summary>
        /// Create DbSet field to read and write do database table "Entry"
        /// </summary>
        public DbSet<Entry> Entries { get; set; }

        /// <summary>
        /// Options for creating
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}