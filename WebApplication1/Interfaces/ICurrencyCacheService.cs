namespace WalletAppication.Interfaces
{
    public interface ICurrencyCacheService
    {
        decimal GetCurrencyConversionRateAsync(string currency);
        void RefreshAllRates();
    }
}
