using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tandem.Users.Api.Models;
using Xunit;

namespace Tandem.Users.Api.IntegrationTests
{
    public class UserIntegrationTests
    {
        private readonly HttpClient _httpClient;

        public UserIntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _httpClient = appFactory.CreateClient();
        }

        [Fact]
        public async Task When_Adding_Valid_User_Expect_Created()
        {
            // TODO: Instead of using Guid email address for uniqueness, I would use the same email address but delete it in test cleanup
            var newUser = new TandemUser() { EmailAddress = new Guid().ToString() + "@.flinstone.com", FirstName = "Fred", LastName = "Flinstone", PhoneNumber = "654-987-1234" };
            var content = new StringContent(newUser.ToString(), null, "application/json");
            var response = await _httpClient.PostAsync("api/v1/user/", content);
            Assert.True(response.StatusCode == HttpStatusCode.Created);
        }

        [Fact]
        public async Task When_Adding_Existing_Email_User_Expect_Conflict()
        {
            // TODO: Instead of using hard-coded email address here, I would use same email from valid create test above AND make sure they run synchronously!!
            var newUser = new TandemUser() { EmailAddress = "aaaa@flinstone.com", FirstName = "Fred", LastName = "Flinstone", PhoneNumber = "654-987-1234" };
            var content = new StringContent(newUser.ToString(), null, "application/json");
            var response = await _httpClient.PostAsync("api/v1/user/", content);
            Assert.True(response.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task When_Existing_Email_Requested_Expect_Success()
        {
            var response = await _httpClient.GetAsync("api/v1/user/aaaa@flinstone.com");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task When_NonExisting_Email_Requested_Expect_NotFound()
        {
            var response = await _httpClient.GetAsync("api/v1/user/bbbb@flinstone.com");
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        // TODO: test for 400 when missing email address

        // TODO: test TandemUserDto fields, especially Name, which concatenates first, middle and last names
    }
}
