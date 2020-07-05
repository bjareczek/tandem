using System.ComponentModel.DataAnnotations;

namespace Tandem.Users.Api.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }

        [Required(ErrorMessage = "EmailAddress Required")]
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
