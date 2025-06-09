using Autofac;
using ECBGateway;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
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
            // Register IDbConnection for SQL Server
            builder.Register(c =>
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                return new SqlConnection(connectionString);
            }).As<IDbConnection>().InstancePerLifetimeScope();
        }
    }
}
