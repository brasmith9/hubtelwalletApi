using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hubtel.Wallets.Api.DAL.DTOs
{
    public class WalletDto
    {
        public int? WalletId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string AccountNumber { get; set; }

        public string Scheme { get; set; }
        public string Owner { get; set; }
    }
}