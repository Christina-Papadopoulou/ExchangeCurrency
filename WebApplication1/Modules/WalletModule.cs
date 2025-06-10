using Autofac;
using ECBGateway;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using WalletAppication.Factories;
using WalletAppication.Interfaces;
using WalletAppication.Repositories;
using WalletAppication.Services;
using WalletApplication.Interfaces;
using WalletApplication.Services;

namespace WalletAppication.Modules
{
    public class WalletModule : Module
    {
        private readonly IConfiguration _configuration;

        public WalletModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WalletService>().As<IWalletService>().InstancePerLifetimeScope();
            builder.RegisterType<ECBClient>().As<IECBClient>().InstancePerLifetimeScope();
            builder.RegisterType<WalletRepository>().As<IWalletRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyRateRepository>().As<ICurrencyRateRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();
            builder.RegisterType<AdjustmentStrategyFactory>().As<IAdjustmentStrategyFactory>().InstancePerLifetimeScope();
            builder.RegisterType<RateLimiterService>().As<IRateLimiterService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyCacheService>().As<ICurrencyCacheService>().InstancePerLifetimeScope();

            // Register IDbConnection for SQL Server
            builder.Register(c =>
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                return new SqlConnection(connectionString);
            }).As<IDbConnection>().InstancePerLifetimeScope();
        }
    }
}
