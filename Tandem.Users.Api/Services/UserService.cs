using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Tandem.Users.Api.Models;

namespace Tandem.Users.Api.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public async Task<User> GetUserById(string userId)
        {
            // TODO: fill out
            return new User();
        }

        public async Task<string> AddUser(User newUser)
        {
            // TODO: fill out to return new userId
            return "abc-xyz";
        }
    }
}
