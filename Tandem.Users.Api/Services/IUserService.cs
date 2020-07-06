using System.Threading.Tasks;
using Tandem.Users.Api.Dtos;
using Tandem.Users.Api.Models;

namespace Tandem.Users.Api.Services
{
    public interface IUserService
    {
        Task<string> AddUser(TandemUser newUser);
        Task<TandemUserDto> GetUserByEmailAddress(string emailAddress);
    }
}