using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Tandem.Users.Api.Dtos;
using Tandem.Users.Api.Settings;

namespace Tandem.Users.Api.Services
{
    public class HealthService : IHealthService
    {
        private readonly ILogger<HealthService> _logger;
        private readonly CosmosDbSettings _cosmosDbSettings;
        private Database _database;
        private Container _container;
        // TODO: I would make this an app level constant
        private const string traceSearchString = "tandem-api-traces :: ";

        public HealthService(ILogger<HealthService> logger, IOptions<CosmosDbSettings> cosmosDbSettings)
        {
            _logger = logger;
            _cosmosDbSettings = cosmosDbSettings.Value;
        }

        public async Task<HealthDto> GetCosmosStatusAsync()
        {
            _logger.LogInformation(traceSearchString + "about to create cosmosClient with endpointUri: " + _cosmosDbSettings.EndpointUri);
            var cosmosClient = new CosmosClient(_cosmosDbSettings.EndpointUri, _cosmosDbSettings.PrimaryKey, new CosmosClientOptions() { ApplicationName = "Tandem.Users.Api.Healthcheck" });
            await CreateDatabaseAsync(cosmosClient);
            await CreateContainerAsync();
            // TODO: I would consider adding and deleting an item for the database for this test in the future.
            if (_container.Id == _cosmosDbSettings.ContainerId)
            {
                // TODO: I would make the CosmosStatus an enum
                return new HealthDto() { CosmosStatus = "ok" };
            }
            // TODO: I would create a monitor in Azure on this following log, and the log string content would be a constant.  I would create an Azure Alert if the monitor was triggered.
            _logger.LogInformation(traceSearchString + "cosmos db not healthy");
            return new HealthDto() { CosmosStatus = "faulty" };
        }

        private async Task CreateDatabaseAsync(CosmosClient cosmosClient)
        {
            _logger.LogInformation(traceSearchString + "about to create cosmos db with databaseId: " + _cosmosDbSettings.DatabaseId);
            _database = await cosmosClient.CreateDatabaseIfNotExistsAsync(_cosmosDbSettings.DatabaseId);
        }

        private async Task CreateContainerAsync()
        {
            _logger.LogInformation(traceSearchString + "about to create cosmos container with containerId: " + _cosmosDbSettings.ContainerId);
            // TODO: I would make this partitionkey a constant
            // TODO: I would make the throughput configurable in appSettings
            // TODO: Still need to add /EmailAddress as unique key during create if the cosmos db does not already exist
            _container = await _database.CreateContainerIfNotExistsAsync(_cosmosDbSettings.ContainerId, "/EmailAddress", 400);
        }
    }
}
