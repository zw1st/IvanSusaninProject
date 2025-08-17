using IvanSusaninProject_Database.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_DataBase.Models;

public class Guide
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Fio { get;  set; }

    public int Experience { get;  set; }

    public int Age { get; set; }

    public string? GuarandorId { get; set; }

    public Executor? Guarantor { get; set; } // изменения для тестов

    [ForeignKey("GuideId")]
    public List<TripGuide>? TripGuides { get; set; }
}