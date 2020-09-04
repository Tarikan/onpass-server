using Microsoft.EntityFrameworkCore;
using onpass_server.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace onpass_server.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Entry> Entries { get; set; }

        public void ApplicationContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(b => b.Email)
                .IsUnique();
            
            modelBuilder.Entity<User>()
                .HasIndex(b => b.Username)
                .IsUnique();
        }
    }
}