using Microsoft.EntityFrameworkCore;
using WalletApplication.Domain;
using WalletApplication.Entities;

namespace WalletAppication.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly IGenericRepository<Wallet> _genericRepository;

        public WalletRepository(IGenericRepository<Wallet> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public Task<Wallet> CreateAsync(Wallet wallet)
        {
            return _genericRepository.CreateAsync(wallet);
        }

        public Task<Wallet> GetByIdAsync(long walletId)
        {
            var wallet = _genericRepository.GetByIdAsync(walletId);
            if (wallet == null)
            {
                throw new KeyNotFoundException($"Wallet with ID {walletId} not found.");
            }

            return wallet;
        }

        public Task UpdateAsync(Wallet wallet)
        {
             return _genericRepository.UpdateAsync(wallet);
        }
    }
}
