using Microsoft.EntityFrameworkCore;
using WalletAppication.Services;
using WalletApplication.Entities;
using WalletApplication.Interfaces;

namespace WalletApplication.Services
{
    public class WalletService : IWalletService
    {
        private readonly AppDbContext _dbContext;
        private readonly CurrencyCacheService _currencyCacheService;

        public WalletService(AppDbContext dbContext, CurrencyCacheService currencyCacheService)
        {
            _dbContext = dbContext;
            _currencyCacheService = currencyCacheService;
        }

        public async Task<Wallet> CreateWalletAsync(decimal initialBalance, string currency)
        {
            var wallet = new Wallet
            {
                Balance = initialBalance,
                Currency = currency
            };

            _dbContext.Wallet.Add(wallet);
            await _dbContext.SaveChangesAsync();
            return wallet;
        }

        public async Task<Wallet> GetWalletBalanceAsync(long walletId, string currency)
        {
            var wallet = await _dbContext.Wallet.FirstOrDefaultAsync(w => w.Id == walletId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            if (wallet.Currency != currency)
            {
                var rate = await _currencyCacheService.GetCurrencyConversionRateAsync(currency);
                wallet.Balance *= rate;
                wallet.Currency = currency;
            }

            return wallet;
        }

        public async Task<Wallet> AdjustWalletBalanceAsync(long walletId, decimal amount, string currency, string strategy)
        {
            var wallet = GetWalletBalanceAsync(walletId, currency).Result;

            if (wallet == null)
                throw new Exception("Wallet not found");

            // Apply strategy for adjusting the wallet balance
            if (strategy == "AddFundsStrategy")
            {
                wallet.Balance += amount;
            }
            else if (strategy == "SubtractFundsStrategy")
            {
                if(wallet.Balance - amount > 0)
                    wallet.Balance -= amount;
            }
            else if (strategy == "ForceSubtractFundsStrategy")
            {
                wallet.Balance -= amount;
            }
            else
            {
                throw new Exception("Unknown strategy");
            }

            await _dbContext.SaveChangesAsync();
            return wallet;
        }
    }
}
