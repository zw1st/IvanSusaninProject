namespace IvanSusaninProject_DataBase.Models;

public class TripPlace
{
    public required string TripId { get; set; }

    public required string PlaceId { get; set; }

    public Trip? Trip { get; set; }

    public Place? Place { get; set; }
}