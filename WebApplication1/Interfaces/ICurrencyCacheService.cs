namespace WalletAppication.Interfaces
{
    public interface ICurrencyCacheService
    {
        Task<decimal> GetCurrencyConversionRateAsync(string currency);
        void RefreshAllRates();
    }
}
