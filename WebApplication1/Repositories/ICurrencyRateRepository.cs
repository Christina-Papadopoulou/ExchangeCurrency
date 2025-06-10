using ECBGateway.Model;
using WalletApplication.Entities;

namespace WalletAppication.Repositories
{
    public interface ICurrencyRateRepository
    {
        Task<List<ECBRate>> GetAllCurrencies();
    }
}
