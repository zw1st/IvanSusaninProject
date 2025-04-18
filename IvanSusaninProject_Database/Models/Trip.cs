using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_DataBase.Models;

public class Trip
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? StartCity { get; set; }

    public string? EndCity { get; set; }

    public DateTime TripDate { get; set; }

    public int Duration { get; set; }

    public required string GuaranderId { get; set; }

    public Guarantor? Guarantor { get; set; }

    [ForeignKey("TripId")]
    public List<TripPlace>? TripPlaces { get; set; }
}
