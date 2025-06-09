using ECBGateway.Model;
using Microsoft.EntityFrameworkCore;
using WalletApplication.Entities;

namespace WalletApplication.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<ECBRate> CurrencyRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet");
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Balance).HasColumnType("decimal(18,2)");
                entity.Property(w => w.Currency).HasMaxLength(10).IsRequired();
            });
            modelBuilder.Entity<ECBRate>(entity =>
            {
                entity.ToTable("CurrencyRates");
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Currency).HasMaxLength(10).IsRequired();
                entity.Property(w => w.Rate).HasColumnType("decimal(18,2)");
                entity.Property(w => w.Date).HasColumnType("datetime").IsRequired();
            });
        }
    }

}
