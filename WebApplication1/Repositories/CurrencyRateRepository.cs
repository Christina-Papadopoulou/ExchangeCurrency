using ECBGateway.Model;
using Microsoft.EntityFrameworkCore;
using WalletApplication.Domain;
using WalletApplication.Entities;

namespace WalletAppication.Repositories
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly IGenericRepository<ECBRate> _genericRepository;

        public CurrencyRateRepository(IGenericRepository<ECBRate> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public Task<List<ECBRate>> GetAllCurrencies()
        {
            return _genericRepository.GetAllAsync();
        }

    }
}
