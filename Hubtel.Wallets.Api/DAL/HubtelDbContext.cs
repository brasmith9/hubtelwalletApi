using Hubtel.Wallets.Api.DAL.DTOs;
using Hubtel.Wallets.Api.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Hubtel.Wallets.Api.DAL
{
    public class HubtelDbContext: IdentityDbContext<AppUser>
    {
        public HubtelDbContext(DbContextOptions options) : base(options)
        {

        }

        public override DbSet<AppUser> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
    }
}