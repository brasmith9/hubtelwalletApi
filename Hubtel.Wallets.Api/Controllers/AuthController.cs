using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hubtel.Wallets.Api.DAL.DTOs;
using Hubtel.Wallets.Api.DAL.Entities;
using Hubtel.Wallets.Api.DTOs.User;
using Hubtel.Wallets.Api.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hubtel.Wallets.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login/{phoneNumber}")]
        public async Task<ActionResult> LoginOrCreateUser(string phoneNumber)
        {
            var response = new OtpResponse { Message = "Otp generated successfully", Status = "success"};

            var otpCode = await _authService.CreateOrUpdateUser(phoneNumber);
            response.Data = otpCode;
            return Ok(response);

        }

        [HttpPost("verifyUser")]
        public async Task<ActionResult<LoginResponse>> VerifyUser([FromBody] LoginDto request)
        {

                var otpResponseObject = await _authService.ValidateOtpAsync(request);

                return Ok(otpResponseObject);


        }

        [HttpPost("userDetails")]
        public async Task<ActionResult<LoginResponse>> UserDetails([FromBody] SignupDto request)
        {

                var userResponse = await _authService.AddUserDetails(request);

                return Ok(userResponse);


        }
    }
}
