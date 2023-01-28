using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Hubtel.Wallets.Api.DAL.Entities
{
    public class AppUser: IdentityUser
    {[Column(TypeName = "varchar(50)")]
        public string FullName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Column(TypeName = "varchar(4)")]
        public string OtpCode { get; set; }

        public DateTime OtpCodeExpiry { get; set; }

        public string City { get; set; }

    }
}