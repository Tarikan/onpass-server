using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace onpass_server.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Entry> Entries { get; set; }

        public void ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=test;Username=test_user;Password=password");
        }
    }
}