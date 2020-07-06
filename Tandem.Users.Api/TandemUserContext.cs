using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tandem.Users.Api.Models;
using Tandem.Users.Api.Settings;

namespace Tandem.Users.Api
{
    public class TandemUserContext: DbContext
    {
        private readonly CosmosDbSettings _cosmosDbSettings;
        public DbSet<TandemUser> TandemUsers { get; set; }

        public TandemUserContext(CosmosDbSettings cosmosDbSettings)
        {
            _cosmosDbSettings = cosmosDbSettings;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos( _cosmosDbSettings.EndpointUri, _cosmosDbSettings.PrimaryKey, _cosmosDbSettings.DatabaseId);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new GuidToStringConverter();
            modelBuilder.Entity<TandemUser>().ToContainer("TandemUsers");
            modelBuilder.Entity<TandemUser>().Property("UserId").HasConversion(converter);
            modelBuilder.Entity<TandemUser>().HasPartitionKey(x => x.EmailAddress);
        }
    }
}
