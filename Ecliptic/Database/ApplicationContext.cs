using Android.InputMethodServices;
using Android.Views;
using Ecliptic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Ecliptic.Repository
{
    public class ApplicationContext : DbContext
    {
        // списки подключенных таблиц
        public DbSet<Client>    Client     { get; set; }
        public DbSet<Note>      Notes    { get; set; }
        public DbSet<FavoriteRoom> FavoriteRooms { get; set; }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor>    Floors    { get; set; }
        public DbSet<Room>     Rooms     { get; set; }
        public DbSet<Worker>   Workers   { get; set; }

        public DbSet<PointM>   Points { get; set; }
        public DbSet<EdgeM>    Edges  { get; set; }

        private string databaseName;

        public ApplicationContext(string databasePath = "database.db")
        {
            databaseName = databasePath;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PointM>()
                              .HasMany(m => m.EdgesIn)
                              .WithOne(t => t.PointTo)
                              .HasForeignKey(m => m.PointToId)
                              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PointM>()
                              .HasMany(m => m.EdgesOut)
                              .WithOne(t => t.PointFrom)
                              .HasForeignKey(m => m.PointFromId)
                              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EdgeM>()
                              .HasOne(m => m.PointTo)
                              .WithMany(t => t.EdgesIn)
                              .HasForeignKey(m => m.PointToId)
                              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EdgeM>()
                              .HasOne(m => m.PointFrom)
                              .WithMany(t => t.EdgesOut)
                              .HasForeignKey(m => m.PointFromId)
                              .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String databasePath =
              Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
          
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}
