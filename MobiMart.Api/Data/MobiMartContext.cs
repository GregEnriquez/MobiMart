using System;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Data;

public class MobiMartContext(DbContextOptions<MobiMartContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Business> Businesses => Set<Business>();
    
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Description> Descriptions => Set<Description>();
    
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<CompletedContract> CompletedContracts => Set<CompletedContract>();
    public DbSet<CompletedContractItem> CompletedContractItems => Set<CompletedContractItem>();

    public DbSet<SalesTransaction> SalesTransactions => Set<SalesTransaction>();
    public DbSet<SalesItem> SalesItems => Set<SalesItem>();

    public DbSet<Reminder> Reminders => Set<Reminder>();
    public DbSet<MonthlyForecastInstance> Forecasts => Set<MonthlyForecastInstance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
