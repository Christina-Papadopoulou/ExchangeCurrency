using WalletApplication.Entities;

namespace WalletApplication.Interfaces
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(decimal initialBalance, string currency);
        Task<Wallet> GetWalletBalanceAsync(long walletId, string currency);
        Task<Wallet> AdjustWalletBalanceAsync(long walletId, decimal amount, string currency, string strategy);
    }
}
