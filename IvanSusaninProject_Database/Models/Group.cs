using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_DataBase.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_Database.Models;

public class Group
{
    public string Id {get; set; } = Guid.NewGuid().ToString();

    public int HumanAmount { get; set;}

    public HumanType HumanType {  get; set;}

    public required string ExecutorId { get; set;}

    public Executor? Executor { get; set; } // изменения для тестов и на будущее

    [ForeignKey("GroupId")]
    public List<TourGroup>? TourGroups { get; set; }
}