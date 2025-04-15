using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IvanSusaninProject_Contracts.Infrastructure;
using System.ComponentModel;
using IvanSusaninProject_Database.Models;


namespace IvanSusaninProject_Database;

internal class IvanSusaninProject_DbContext(IConfigurationDatabase configurationDatabase) : DbContext
{
    private readonly IConfigurationDatabase? _configurationDatabase =
    configurationDatabase;

    protected override void OnConfiguring(DbContextOptionsBuilder
    optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configurationDatabase?.ConnectionString, o
        => o.SetPostgresVersion(12, 2));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Excursion>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<Executor>().HasIndex(x => new { x.Login, x.Password }).IsUnique();
        modelBuilder.Entity<Tour>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<TourExcursion>().HasIndex(x => new { x.TourId, x.ExcursionId });
        modelBuilder.Entity<TourGroup>().HasIndex(x => new { x.TourId, x.GroupId });
    }

    public DbSet<Excursion> SExcursions { get; set; }
    public DbSet<Executor> Executors { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourExcursion> TourExcursions { get; set; }
    public DbSet<TourGroup> TourGroups { get; set; }
}
