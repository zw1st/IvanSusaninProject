using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_DataBase.Models;

public class Place
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? Address { get; set; }

    public string? City { get; set; }

    public required string Name { get; set; }

    public required string GroupId { get; set; }

    public required string GuaranderId { get; set; }

    public Group? Group { get; set; }

    public Guarantor? Guarantor { get; set; }
}