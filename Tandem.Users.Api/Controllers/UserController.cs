using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Tandem.Users.Api.Dtos;
using Tandem.Users.Api.Models;
using Tandem.Users.Api.Services;

namespace Tandem.Users.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        // TODO: I would make this an app level constant
        private const string traceSearchString = "tandem-api-traces :: ";

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("{emailAddress}", Name = "GetUserByEmailAddress")]
        public async Task<IActionResult> GetUserByEmailAddress(string emailAddress)
        {
            _logger.LogInformation(traceSearchString + "about to get user with emailAddress: " + emailAddress);
            var user = await _userService.GetUserByEmailAddress(emailAddress);
            if(user == null) 
            {
                _logger.LogInformation(traceSearchString + "did not find user with emailAddress: " + emailAddress);
                return NotFound(); 
            }
            _logger.LogInformation(traceSearchString + "about to return user with emailAddress: " + user.EmailAddress);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TandemUser newUser)
        {
            _logger.LogInformation(traceSearchString + "about to add new user with following payload: ");
            var addUserResponse = await _userService.AddUser(newUser);
            // TODO: like in service, this response would be from a constant
            if (addUserResponse == "duplicateEmail") return Conflict();
            _logger.LogInformation(traceSearchString + "added new user with userId: " + newUser.UserId);
            return CreatedAtRoute("GetUserByEmailAddress", new { emailAddress = newUser.EmailAddress }, new TandemUserDto(newUser));
        }
    }
}
