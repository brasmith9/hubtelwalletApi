using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hubtel.Wallets.Api.DAL;
using Hubtel.Wallets.Api.DAL.DTOs;
using Hubtel.Wallets.Api.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallets.Api.Services
{
    public class WalletService: IWalletService
    {
        private readonly HubtelDbContext _context;

        public WalletService(HubtelDbContext context)
        {
            _context = context;
        }

        private async Task<bool> CheckForDuplicateWallet(string accountNumber)
        {
           var wallets = await _context.Wallets.CountAsync(wallet => wallet.AccountNumber == accountNumber.Trim());

           if (wallets > 0) return true;
           return false;
        }


        private async Task<bool> UserCanAddAnotherWallet(string phoneNumber)
        {

            var wallets = await _context.Wallets.CountAsync(wallet => wallet.Owner == phoneNumber.Trim());

            return wallets < 5;
        }

        private static bool IsValidScheme(string scheme, string type)
        {
            scheme = scheme.ToLower().Trim();

            switch (type.ToLower().Trim())
            {
                case "momo":
                    if (scheme == "mtn" || scheme == "vodafone" || scheme == "airteltigo") return true;
                    break;
                case "card":
                    if (scheme == "visa" || scheme == "mastercard") return true;
                    break;
                default:
                    return false;
            }

            return false;

        }

        public async Task<AddWalletResponse> AddWalletToUserAccount(WalletDto walletDto)
        {
            var response = new AddWalletResponse { Message = "", Status = "fail"};

            if (await CheckForDuplicateWallet(walletDto.AccountNumber))
            {
                response.Message = "Wallet with this Account Number exists.";
                return response;
            }

            if (!await UserCanAddAnotherWallet(walletDto.Owner))
            {
                response.Message = "You have reached the maximum number of Wallets";
                return response;
            }

            if (!IsValidScheme(walletDto.Scheme, walletDto.Type))
            {
                response.Message = "Invalid Scheme and Type Combination";
                return response;
            }

            if (walletDto.Type != "momo")
            {
                walletDto.AccountNumber = walletDto.AccountNumber.Substring(0, 6);
            }
            var wallet = new Wallet { Name = walletDto.Name, Type = walletDto.Type, Scheme = walletDto.Scheme, AccountNumber = walletDto.AccountNumber, Owner = walletDto.Owner };
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            response.Message = "Wallet added successfully";
            response.Status = "success";
            return response;
        }

        public async Task<AddWalletResponse> RemoveWalletFromUserAccount(int walletId)
        {
            var response = new AddWalletResponse { Message = "", Status = "fail"};
            var wallet = await _context.Wallets.FirstOrDefaultAsync(wallet => wallet.WalletId == walletId);
            if (wallet == null)
            {
                response.Message = "Invalid Wallet ID";
                return response;
            }

            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
            response.Status = "success";
            response.Message = "Wallet deleted successfully";

            return response;
        }

        public async Task<List<Wallet>> GetAllUserWallet(string owner)
        {
            return await _context.Wallets.Where(wallet => wallet.Owner == owner).ToListAsync();
        }

        public async Task<Wallet> GetUserWalletById(int id, string owner)
        {
            return await _context.Wallets.SingleOrDefaultAsync(wallet => (wallet.WalletId == id && wallet.Owner == owner));
        }
    }
}