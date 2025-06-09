// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System;

Console.WriteLine("Hello, World!");

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlServer("Server=.;Database=TestDb;Trusted_Connection=True;");

using var context = new AppDbContext(optionsBuilder.Options);
var wallets = context.Wallets.ToList();
Console.WriteLine(wallets.Count);


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Wallet> Wallets { get; set; }
}

public class Wallet
{
    public long Id { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = string.Empty;
}