using System.Threading.Tasks;
using Hubtel.Wallets.Api.DAL.DTOs;
using Hubtel.Wallets.Api.DTOs.User;

namespace Hubtel.Wallets.Api.Interfaces
{
    public interface IAuthService
    {
        public Task<string> CreateOrUpdateUser(string phoneNumber);

        public Task<object> ValidateOtpAsync(LoginDto loginDto);

        public Task<SignupResponse> AddUserDetails(SignupDto signupDto);
    }
}