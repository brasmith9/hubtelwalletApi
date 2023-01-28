using System;

namespace Hubtel.Wallets.Api.DTOs.User
{
    public class SignupDto
    {

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string City { get; set; }
    }
}