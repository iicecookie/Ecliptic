using Ecliptic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ecliptic.Repository
{
    public class ApplicationContext : DbContext
    {
        private string databaseName;

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Worker> Workers { get; set; }

        public DbSet<User> User  { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public ApplicationContext(string databasePath = "database.db")
        {
            databaseName = databasePath;
        }
        //    protected override void OnModelCreating(ModelBuilder builder)
        //    {
        //        base.OnModelCreating(builder);
        //
        //        // Define composite key.
        //        //     builder.Entity<User>().HasMany(p => p.Notes).WithOne(c => c.Owner);
        //    }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String databasePath =
              Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);

            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}
