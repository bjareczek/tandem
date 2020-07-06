using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tandem.Users.Api.Models;

namespace Tandem.Users.Api
{
    public class TandemUserContext: DbContext
    {
        public DbSet<TandemUser> TandemUsers { get; set; }

        public TandemUserContext(DbContextOptions<TandemUserContext> options) : base(options)
        {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new GuidToStringConverter();
            modelBuilder.Entity<TandemUser>().ToContainer("TandemUsers");
            modelBuilder.Entity<TandemUser>().Property("UserId").HasConversion(converter);
            modelBuilder.Entity<TandemUser>().HasPartitionKey(x => x.EmailAddress);
        }
    }
}
