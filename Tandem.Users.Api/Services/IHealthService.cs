using System.Threading.Tasks;
using Tandem.Users.Api.Dtos;

namespace Tandem.Users.Api.Services
{
    public interface IHealthService
    {
        Task<HealthDto> GetCosmosStatusAsync();
    }
}