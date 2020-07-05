using System.Threading.Tasks;
using Tandem.Users.Api.Models;

namespace Tandem.Users.Api.Services
{
    public interface IUserService
    {
        Task<string> AddUser(User newUser);
        Task<User> GetUserById(string userId);
    }
}