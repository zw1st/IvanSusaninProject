using IvanSusaninProject_Contracts.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_Database.Models;

public class Group
{
    public required string Id {get; set; } = Guid.NewGuid().ToString();

    public int HumanAmount { get; set;}

    public HumanType HumanType {  get; set;}

    public required string ExecutorId { get; set;}

    public Executor? Executor { get; set; }

    [ForeignKey("GroupId")]
    public List<TourGroup>? TourGroups { get; set; }
}
