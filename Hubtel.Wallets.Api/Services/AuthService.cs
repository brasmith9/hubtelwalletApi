#nullable enable
using System;
using System.Threading.Tasks;
using Hubtel.Wallets.Api.DAL.Entities;
using Hubtel.Wallets.Api.DTOs.User;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallets.Api.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<SignupResponse> AddUserDetails(SignupDto signupDto)
        {
            var response = new SignupResponse { Message = "", Status = "fail"  };
            var appUser = await GetUserFromPhoneNumber(signupDto.PhoneNumber);
            if (appUser is null)
            {
                response.Message = "No user exists for this account";
                return response;
            }

            appUser.FullName = signupDto.FullName.Trim();
            appUser.City = signupDto.City.Trim();
            appUser.Gender = signupDto.Gender.Trim();
            appUser.DateOfBirth = signupDto.DateOfBirth;
            appUser.Email = signupDto.Email;

           await _userManager.UpdateAsync(appUser);

           response.Status = "success";
           response.Message = "User details updated successfully";

            return response;
        }


        public async Task<string> CreateOrUpdateUser(string phoneNumber)
        {
            var appUser = await GetUserFromPhoneNumber(phoneNumber);
            var pin = Generators.GenerateRandomPin(4);
            var pinExpiry = DateTime.UtcNow.AddMinutes(30);

            if (appUser is null)
            {
                appUser = new AppUser
                {
                    Email = "", UserName = phoneNumber, PhoneNumber = phoneNumber, OtpCode = pin,
                    OtpCodeExpiry = pinExpiry
                };
                await _userManager.CreateAsync(appUser);
                return pin;
            }

            appUser.OtpCode = pin;
            appUser.OtpCodeExpiry = pinExpiry;

            await _userManager.UpdateAsync(appUser);

            //Logic to send OTP via SMS goes here
            return pin;
        }

        public async Task<object> ValidateOtpAsync(LoginDto loginData)
        {
            var response = new LoginResponse { Message = "", Status = "fail" };
            var user = await _userManager.Users.SingleOrDefaultAsync(user =>
                user.PhoneNumber.Equals(loginData.PhoneNumber.Trim()));

            if (user.OtpCodeExpiry < DateTime.Now)
            {
                response.Message = "Otp is invalid";
                return response;
            }

            if (user.OtpCode != loginData.OtpCode)
            {
                response.Message = "Otp is invalid please try again";
                return response;
            }

            var jwt = await TokenService.GenerateTokenAsync(_userManager, user);

            user.OtpCode = null;

            await _userManager.UpdateAsync(user);

            response.Data = jwt;
            response.Status = "Success";
            response.Message = "Successfully authenticated";

            return response;

        }

        private async Task<AppUser?> GetUserFromPhoneNumber(string phoneNumber)
        {
            return await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber.Equals(phoneNumber));
        }


    }
}