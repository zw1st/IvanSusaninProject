using IvanSusaninProject_DataBase.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_Database.Models;

public class Excursion
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Name { get; set; }

    public DateTime ExcursionDate { get; set; }

    public required string ExecutorId { get; set; }

    public Executor? Executor { get; set; }

    public string? GuideId { get; set; }

    public Guide? Guide { get; set; }

    [ForeignKey("ExcursionId")]
    public List<TourExcursion>? TourExcursions { get; set; }
}