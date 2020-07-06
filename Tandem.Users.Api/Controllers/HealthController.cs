using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Tandem.Users.Api.Services;

namespace Tandem.Users.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IHealthService _healthService;
        // TODO: I would make this an app level constant
        private const string traceSearchString = "tandem-api-traces :: ";

        public HealthController(ILogger<UserController> logger, IHealthService healthService)
        {
            _logger = logger;
            _healthService = healthService;
        }

        [HttpGet]
        public async Task<HealthCheckResult> GetCosmosDbHealth()
        {
            _logger.LogInformation(traceSearchString + "about to get cosmos db health");
            var health = await _healthService.GetCosmosStatusAsync();
            var healthResult = health.CosmosStatus == "ok" ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
            return healthResult;
        }
    }
}
