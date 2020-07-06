using Newtonsoft.Json;
using System;
using Tandem.Users.Api.Models;

namespace Tandem.Users.Api.Dtos
{
    public class TandemUserDto
    {
        public TandemUserDto(TandemUser tandemUser)
        {
            if (tandemUser == null) return;
            UserId = tandemUser.UserId;
            EmailAddress = tandemUser.EmailAddress;
            Name = ((tandemUser.FirstName?.Trim() + " " + tandemUser.MiddleName?.Trim()).Trim() + " " + tandemUser.LastName?.Trim()).Trim();
            PhoneNumber = tandemUser.PhoneNumber;
        }

        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
