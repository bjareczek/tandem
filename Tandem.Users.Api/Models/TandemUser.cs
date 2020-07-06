using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Tandem.Users.Api.Models
{
    public class TandemUser
    {
        [Key]
        public Guid UserId { get; set; }
        public string id { get; set; }
        [Required(ErrorMessage = "EmailAddress Required")]
        // TODO: add email format validation
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
