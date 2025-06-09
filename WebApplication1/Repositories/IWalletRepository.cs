using WalletApplication.Entities;

namespace WalletAppication.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet> CreateAsync(Wallet wallet);
        Task<Wallet> GetByIdAsync(long walletId);
        Task UpdateAsync(Wallet wallet);
    }
}
