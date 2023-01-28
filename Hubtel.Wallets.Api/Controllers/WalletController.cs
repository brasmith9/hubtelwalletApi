using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hubtel.Wallets.Api.DAL;
using Hubtel.Wallets.Api.DAL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Hubtel.Wallets.Api.DAL.Entities;
using Hubtel.Wallets.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Hubtel.Wallets.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetWallets()
        {
            ClaimsPrincipal authUser = User;
            var wallets = await _walletService.GetAllUserWallet(authUser.Identity.Name);
            return Ok(wallets);
        }

        [HttpGet("{walletId}")]
        public async Task<ActionResult> GetWallet(int walletId)
        {

            ClaimsPrincipal authUser = User;
            var wallet = await _walletService.GetUserWalletById(walletId, authUser.Identity.Name);
            return Ok(wallet);
        }


        [HttpPost]
        public async Task<ActionResult<WalletDto>> CreateWallet(WalletDto walletDto)
        {
            ClaimsPrincipal authUser = User;

            walletDto.Owner = authUser.Identity.Name;
            var wallet = await _walletService.AddWalletToUserAccount(walletDto);
            if (wallet.Status == "fail")
            {
                return BadRequest(wallet);
            }

            return Created("/", wallet);
        }

        [HttpDelete("{walletId}")]
        public async Task<ActionResult> DeleteWallet(int walletId)
        {

            var response = await _walletService.RemoveWalletFromUserAccount(walletId);

            if (response.Status == "fail")
            {
                return BadRequest(response);
            }
            return NoContent();
        }
    }
}
