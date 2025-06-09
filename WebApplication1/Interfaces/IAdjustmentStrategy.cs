using WalletApplication.Entities;

namespace WalletAppication.Interfaces
{
    public interface IAdjustmentStrategy
    {
        void AdjustBalance(Wallet wallet, decimal amount);
    }
}
