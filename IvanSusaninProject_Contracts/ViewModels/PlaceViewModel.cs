namespace IvanSusaninProject_Contracts.ViewModels;

public class PlaceViewModel
{
    public required string Id { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public required string Name { get; set; }

    public string? GroupId { get; set; }

    public required string GuarandorId { get; set; }

    public required string GuarantorLogin { get; set; }
}