using WalletAppication.Helpers;
using WalletAppication.Interfaces;

namespace WalletAppication.Factories
{
    public class AdjustmentStrategyFactory : IAdjustmentStrategyFactory
    {
        public IAdjustmentStrategy Create(string strategy)
        {
            return strategy switch
            {
                "AddFundsStrategy" => new AddFundsStrategy(),
                "SubtractFundsStrategy" => new SubtractFundsStrategy(),
                "ForceSubtractFundsStrategy" => new ForceSubtractFundsStrategy(),
                _ => throw new ArgumentException("Invalid strategy", nameof(strategy)),
            };
        }
    }
}
