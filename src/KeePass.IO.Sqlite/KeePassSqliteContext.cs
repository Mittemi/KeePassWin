﻿using Microsoft.Data.Entity;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KeePass
{
    public class KeePassSqliteContext : DbContext
    {
        public DbSet<DatabaseEntry> KeePassDatabases { get; set; }
        public DbSet<File> Files { get; set; }

        static KeePassSqliteContext()
        {
            Debug.WriteLine($@"Sqlite database will be at {Windows.Storage.ApplicationData.Current.LocalFolder.Path}\settings.dat");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename=settings.dat");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .Property(f => f.Path)
                .IsRequired();
        }

        public static async Task InitializeAsync()
        {
            using (var db = new KeePassSqliteContext())
            {
                await db.Database.MigrateAsync();
            }
        }
    }

    public class DatabaseEntry
    {
        public int Id { get; set; }
        public virtual File KeePass { get; set; }
        public virtual File Key { get; set; }
    }

    public class File
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string AccessToken { get; set; }
    }
}