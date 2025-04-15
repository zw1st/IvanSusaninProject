using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Database.Models;

internal class Excursion
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Name { get; set; }

    public DateTime ExcursionDate { get; set; }

    public required string ExecutorId { get; set; }

    public Executor? Executor { get; set; }

    public required string GuideId { get; set; }

    public Guide? Guide { get; set; }

    [ForeignKey("ExcursionId")]
    public List<TourExcursion>? TourExcursions { get; set; }
}
