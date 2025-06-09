using WalletAppication.Interfaces;
using WalletAppication.Repositories;
using WalletAppication.Services;
using WalletApplication.Domain;
using WalletApplication.Entities;
using WalletApplication.Interfaces;

namespace WalletApplication.Services
{
    public class WalletService : IWalletService
    {
        private readonly CurrencyCacheService _currencyCacheService;
        private readonly IAdjustmentStrategyFactory _strategyFactory;
        private readonly IWalletRepository _walletRepository;

        public WalletService(CurrencyCacheService currencyCacheService, IAdjustmentStrategyFactory strategyFactory, IWalletRepository walletRepository)
        {
            _currencyCacheService = currencyCacheService;
            _strategyFactory = strategyFactory;
            _walletRepository = walletRepository;
        }

        public async Task<Wallet> CreateWalletAsync(decimal initialBalance, string currency)
        {
            var wallet = new Wallet
            {
                Balance = initialBalance,
                Currency = currency
            };

            return await _walletRepository.CreateAsync(wallet);
        }

        public async Task<Wallet> GetWalletBalanceAsync(long walletId, string currency)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);

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

            // Get the correct adjustment strategy via the factory
            var adjustmentStrategy = _strategyFactory.Create(strategy);

            // Apply the strategy to adjust the wallet balance
            adjustmentStrategy.AdjustBalance(wallet, amount);

            // Update the wallet in the database
            await _walletRepository.UpdateAsync(wallet);

            return wallet;
        }
    }
}
