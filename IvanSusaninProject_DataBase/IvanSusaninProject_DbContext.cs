
using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Database.Models;
using Microsoft.EntityFrameworkCore;

namespace IvanSusaninProject_DataBase;

public class IvanSusaninProject_DbContext : DbContext
{
    private readonly IConfigurationDatabase? _configurationDatabase;

    public IvanSusaninProject_DbContext(IConfigurationDatabase? configurationDatabase)
    {
        _configurationDatabase = configurationDatabase;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configurationDatabase?.ConnectionString, o
        => o.SetPostgresVersion(12, 2));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Guarantor>().HasIndex(e => new { e.Login, e.Password }).IsUnique();
        modelBuilder.Entity<Guide>().HasIndex(e => new { e.Fio}).IsUnique();
        modelBuilder.Entity<Place>().HasIndex(x => new { x.Name }).IsUnique();
        modelBuilder.Entity<TripGuide>().HasKey(x => new {x.TripId, x.GuideId});
        modelBuilder.Entity<TripPlace>().HasKey(x => new { x.TripId, x.PlaceId });
    }

    public DbSet<Guarantor> Guarantors { get; set; }

    public DbSet<Guide> Guides { get; set; }

    public DbSet<Trip> Trips { get; set; }

    public DbSet<Place> Places { get; set; }

    public DbSet<TripGuide> TripGuides { get; set; }

    public DbSet<TripPlace> TripPlaces { get; set; }
}