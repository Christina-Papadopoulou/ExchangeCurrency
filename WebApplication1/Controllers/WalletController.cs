using Microsoft.AspNetCore.Mvc;
using WalletAppication.Attributes;
using WalletAppication.Models;
using WalletApplication.Interfaces;

namespace WalletApplication.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        // Create Wallet
        [HttpPost]
        [RateLimit("wallet/createwallet")]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest request)
        {
            try
            {
                var wallet = await _walletService.CreateWalletAsync(request.InitialBalance, request.Currency);

                return CreatedAtAction(nameof(GetWalletBalance), new { walletId = wallet.Id }, wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Retrieve Wallet Balance
        [HttpGet("{walletId}")]
        [RateLimit("wallet/getbalance")]
        public async Task<IActionResult> GetWalletBalance(long walletId, [FromQuery] string currency)
        {
            try
            {
                var wallet = await _walletService.GetWalletBalanceAsync(walletId, currency);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Adjust Wallet Balance
        [HttpPost("{walletId}/adjustbalance")]
        [RateLimit("wallet/adjustbalance")]
        public async Task<IActionResult> AdjustWalletBalance(long walletId, [FromQuery] decimal amount, [FromQuery] string currency, [FromQuery] string strategy)
        {
            try
            {
                var wallet = await _walletService.AdjustWalletBalanceAsync(walletId, amount, currency, strategy);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
