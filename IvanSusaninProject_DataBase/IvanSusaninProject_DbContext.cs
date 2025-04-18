using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IvanSusaninProject_Contracts.Infrastructure;
using System.ComponentModel;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Models;


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
        modelBuilder.Entity<Guarantor>().HasIndex(e => new { e.Login, e.Password }).IsUnique();
        modelBuilder.Entity<Guide>().HasIndex(e => new { e.Fio }).IsUnique();
        modelBuilder.Entity<Place>().HasIndex(x => new { x.Name }).IsUnique();
        modelBuilder.Entity<TripGuide>().HasKey(x => new { x.TripId, x.GuideId });
        modelBuilder.Entity<TripPlace>().HasKey(x => new { x.TripId, x.PlaceId });
    }

    public DbSet<Excursion> Excursions { get; set; }
    public DbSet<Executor> Executors { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourExcursion> TourExcursions { get; set; }
    public DbSet<TourGroup> TourGroups { get; set; }
    public DbSet<Guarantor> Guarantors { get; set; }
    public DbSet<Guide> Guides { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<TripGuide> TripGuides { get; set; }
    public DbSet<TripPlace> TripPlaces { get; set; }
}
