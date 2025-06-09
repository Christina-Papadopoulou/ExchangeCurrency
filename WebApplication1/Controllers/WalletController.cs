using Microsoft.AspNetCore.Mvc;
using WalletAppication.Models;
using WalletApplication.Interfaces;

namespace WalletApplication.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(ILogger<WalletController> logger, IWalletService walletService)
        {
            _logger = logger;
            _walletService = walletService;
        }

        // Create Wallet
        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest request)
        {
            var wallet = await _walletService.CreateWalletAsync(request.InitialBalance, request.Currency);
            return CreatedAtAction(nameof(GetWalletBalance), new { walletId = wallet.Id }, wallet);
        }

        // Retrieve Wallet Balance
        [HttpGet("{walletId}")]
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
