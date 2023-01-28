using System.Collections.Generic;
using System.Threading.Tasks;
using Hubtel.Wallets.Api.DAL.DTOs;
using Hubtel.Wallets.Api.DAL.Entities;

namespace Hubtel.Wallets.Api.Services
{
    public interface IWalletService
    {
        public Task<AddWalletResponse> AddWalletToUserAccount(WalletDto walletDto);

        public Task<AddWalletResponse> RemoveWalletFromUserAccount(int walletId);

        public Task<List<Wallet>> GetAllUserWallet(string owner);

        public Task<Wallet> GetUserWalletById(int id, string owner);

    }

}