using Microsoft.EntityFrameworkCore;

namespace Wallet
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Entities.Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Wallet>(entity =>
            {
                entity.ToTable("Wallets");
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Balance).HasColumnType("decimal(18,2)");
                entity.Property(w => w.Currency).HasMaxLength(10).IsRequired();
            });
        }
    }
}
