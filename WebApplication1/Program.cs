using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WalletAppication.Modules;
using WalletAppication.Services;
using WalletApplication;
using WalletApplication.Jobs;

var builder = WebApplication.CreateBuilder(args);

// 1. Register HttpClient using IHttpClientFactory
builder.Services.AddHttpClient();

// 2. Register DbContext (for EF Core)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Set up Autofac container and register modules
var configuration = builder.Configuration;
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new WalletModule(configuration));
    });

// 4. Register controllers, Swagger, etc.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. Register MemoryCache and CurrencyCacheService
builder.Services.AddMemoryCache(); 
builder.Services.AddScoped<CurrencyCacheService>(); 

// 5. Register the background service for periodic tasks.
builder.Services.AddHostedService<UpdateRatesPeriodically>();

var app = builder.Build();

// 6.Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Ensure the database is created and migrations are applied
}

// 7. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
