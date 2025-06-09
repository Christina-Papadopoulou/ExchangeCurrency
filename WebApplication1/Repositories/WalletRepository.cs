using Microsoft.EntityFrameworkCore;
using WalletApplication.Domain;
using WalletApplication.Entities;

namespace WalletAppication.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _dbContext;

        public WalletRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Wallet> CreateAsync(Wallet wallet)
        {
            _dbContext.Wallet.Add(wallet);
            await _dbContext.SaveChangesAsync();
            return wallet;
        }

        public async Task<Wallet> GetByIdAsync(long walletId)
        {
            var wallet = await _dbContext.Wallet.FirstOrDefaultAsync(w => w.Id == walletId);
            if (wallet == null)
            {
                throw new KeyNotFoundException($"Wallet with ID {walletId} not found.");
            }

            return wallet;
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            _dbContext.Wallet.Update(wallet);
            await _dbContext.SaveChangesAsync();
        }
    }
}
