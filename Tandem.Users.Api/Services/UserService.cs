using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Users.Api.Dtos;
using Tandem.Users.Api.Models;
using Tandem.Users.Api.Settings;

namespace Tandem.Users.Api.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly CosmosDbSettings _cosmosDbSettings;
        // TODO: I would make this an app level constant
        private const string traceSearchString = "tandem-api-traces :: ";

        public UserService(ILogger<UserService> logger, IOptions<CosmosDbSettings> cosmosDbSettings)
        {
            _logger = logger;
            _cosmosDbSettings = cosmosDbSettings.Value;
        }

        public async Task<TandemUserDto> GetUserByEmailAddress(string emailAddress)
        {
            _logger.LogInformation(traceSearchString + "about to get user from cosmos with emailAddress: " + emailAddress);
            using (var context = new TandemUserContext(_cosmosDbSettings))
            {
                var tandemUser = await context.TandemUsers.Where(tu => tu.EmailAddress == emailAddress).FirstOrDefaultAsync();
                if (tandemUser?.id == null)
                {
                    _logger.LogInformation(traceSearchString + "did not find user from cosmos with emailAddress: " + emailAddress);
                    return null;
                }
                _logger.LogInformation(traceSearchString + "retrieved user from cosmos with emailAddress: " + tandemUser.EmailAddress);
                var tandemUserDto = new TandemUserDto(tandemUser);
                return tandemUserDto;
            }
        }

        public async Task<string> AddUser(TandemUser newUser)
        {
            _logger.LogInformation(traceSearchString + "about to add user to cosmos with emailAddress: " + newUser.EmailAddress);
            using (var context = new TandemUserContext(_cosmosDbSettings))
            {
                context.Database.EnsureCreated();
                newUser.UserId = Guid.NewGuid();
                newUser.id = "TandemUser|" + newUser.UserId.ToString();
                context.Add(newUser);
                await context.SaveChangesAsync();
            }
            _logger.LogInformation(traceSearchString + "add user to cosmos with Id: " + newUser?.UserId);
            return newUser.UserId.ToString();
        }
    }
}
