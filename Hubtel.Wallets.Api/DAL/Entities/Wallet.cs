using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hubtel.Wallets.Api.DAL.Entities
{
    public class Wallet
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WalletId { get; set; }

        [Required, Column(TypeName="varchar(50)")]
        public string Name { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public string Type { get; set; }

        [Required, Column(TypeName= "varchar(50)")]
        public string AccountNumber { get; set; }

        [Required, Column(TypeName= "varchar(50)")]
        public string Scheme { get; set; }

        [Required, Column(TypeName= "varchar(50)")]
        public string Owner { get; set; }


    }
}