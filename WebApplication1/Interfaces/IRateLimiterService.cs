namespace WalletAppication.Interfaces
{
    public interface IRateLimiterService
    {
        bool IsRateLimited(string ipAddress, string endpoint);
    }
}
