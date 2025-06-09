using Dapper;
using ECBGateway;
using ECBGateway.Model;
using System.Data;
using System.Net;

namespace Wallet.Jobs
{
    public class UpdateRatesPeriodically : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<UpdateRatesPeriodically> _logger;

        public UpdateRatesPeriodically(IServiceProvider services, ILogger<UpdateRatesPeriodically> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();
                var client = scope.ServiceProvider.GetRequiredService<IECBClient>();
                var rates = await client.GetLatestRatesAsync();

                var db = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                await UpsertRatesAsync(db, rates);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task UpsertRatesAsync(IDbConnection db, IEnumerable<ECBRate> rates)
        {
            var date = rates.First().Date;
            var sql = @"
MERGE INTO ExchangeRates AS target
USING (VALUES " + string.Join(",", rates.Select((r, i) =>
    $"(@Currency{i}, @Rate{i}, @Date{i})")) + @") AS source (Currency, Rate, Date)
ON target.Currency = source.Currency AND target.Date = source.Date
WHEN MATCHED THEN
    UPDATE SET Rate = source.Rate
WHEN NOT MATCHED THEN
    INSERT (Currency, Rate, Date) VALUES (source.Currency, source.Rate, source.Date);";

            var parameters = new DynamicParameters();
            int index = 0;
            foreach (var rate in rates)
            {
                parameters.Add($"Currency{index}", rate.Currency);
                parameters.Add($"Rate{index}", rate.Rate);
                parameters.Add($"Date{index}", rate.Date);
                index++;
            }

            await db.ExecuteAsync(sql, parameters);
        }
    }
}
