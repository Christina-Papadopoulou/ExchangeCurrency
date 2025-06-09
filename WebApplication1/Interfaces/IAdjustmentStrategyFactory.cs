namespace WalletAppication.Interfaces
{
    public interface IAdjustmentStrategyFactory
    {
        IAdjustmentStrategy Create(string strategy);
    }
}
