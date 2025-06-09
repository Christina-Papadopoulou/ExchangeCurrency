using WalletAppication.Interfaces;
using WalletApplication.Entities;

namespace WalletAppication.Helpers
{
    public class AddFundsStrategy : IAdjustmentStrategy
    {
        public void AdjustBalance(Wallet wallet, decimal amount)
        {
            wallet.Balance += amount;
        }
    }
}
