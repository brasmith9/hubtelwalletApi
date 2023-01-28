using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hubtel.Wallets.Api.DTOs.User;
using Hubtel.Wallets.Api.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Hubtel.Wallets.Api.Services
{
    public class TokenService
    {
        private const string ValidAudience = "User";
        private const string ValidIssuer = "";
        private const string TokenKey = "M1cr0T0kn!!Th@t1sUbreK@b1eByEu$0RY0uDouBt";
        private const double TokenExpiry = 10.00;

        public static string ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authSigningkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = authSigningkey,
                ValidIssuer = ValidIssuer,
                ValidAudience = ValidAudience,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true
            };

            var valid = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            if (valid == null) return null;

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;


            return userId;
        }

        public static async Task<Jwt> GenerateTokenAsync(UserManager<AppUser> userManager, AppUser appUser)
        {

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier,appUser.Id.ToString()),
                new Claim(ClaimTypes.Name, appUser.PhoneNumber),
            };

            var userRoles = await userManager.GetRolesAsync(appUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }


            var authSigningkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey));
            var tokenOptions = new JwtSecurityToken(
                    issuer: ValidIssuer,
                    audience: ValidAudience,
                    expires: DateTime.Now.AddHours(TokenExpiry),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningkey, SecurityAlgorithms.HmacSha512)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new Jwt { token = token, userId = appUser.Id, username = appUser.UserName };         }
    }
}