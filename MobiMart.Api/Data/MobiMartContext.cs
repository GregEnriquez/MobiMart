using System;
using Microsoft.EntityFrameworkCore;
using MobiMart.Api.Entities;

namespace MobiMart.Api.Data;

public class MobiMartContext(DbContextOptions<MobiMartContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Description> Descriptions => Set<Description>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
