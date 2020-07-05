using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Tandem.Users.Api.Models;
using Tandem.Users.Api.Services;

namespace Tandem.Users.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private const string userApiPath = "api/v1/user/{0}";
        // TODO: I would make this an app level constant
        private const string traceSearchString = "tandem-api-traces :: ";

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            _logger.LogInformation(traceSearchString + "about to get user with userId: " + userId);
            var user = await _userService.GetUserById(userId);
            _logger.LogInformation(traceSearchString + "about to return user with userId: " + user.UserId);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User newUser)
        {
            _logger.LogInformation(traceSearchString + "about to add new user with following payload: ");
            var newUserId = await _userService.AddUser(newUser);
            var newUserUri = string.Format(userApiPath, newUserId);
            _logger.LogInformation(traceSearchString + "added new user with uri: " + newUserUri);
            return Ok(newUserUri);
        }
    }
}
