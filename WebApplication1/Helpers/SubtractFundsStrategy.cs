using WalletAppication.Interfaces;
using WalletApplication.Entities;

namespace WalletAppication.Helpers
{
    public class SubtractFundsStrategy : IAdjustmentStrategy
    {
        public void AdjustBalance(Wallet wallet, decimal amount)
        {
            if (wallet.Balance - amount >= 0)
            {
                wallet.Balance -= amount;
            }
            else
            {
                throw new Exception("Insufficient funds for this operation.");
            }
        }
    }
}
