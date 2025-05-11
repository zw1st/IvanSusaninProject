using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_Database.Models;

public class Tour
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Name { get; set; }

    public string? City { get; set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public required string ExecutorId { get; set; }

    public Executor? Executor { get; set; }

    [ForeignKey("TourId")]
    public List<TourGroup>? TourGroups { get; set; }

    [ForeignKey("TourId")]
    public List<TourExcursion>? TourExcursions { get; set; }
}
