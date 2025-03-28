
namespace IvanSusaninProject_Contracts.DataModels;

public class PlaceDataModel(string id, string address, string city, string name, string groupId, string guaranderId)
{
    public string Id { get; private set; } = id;

    public string Address { get; private set; } = address;

    public string City { get; private set; } = city;

    public string Name { get; private set; } = name;

    public string GroupId { get; private set; } = groupId;

    public string GuaranderId { get; private set; } = guaranderId;
}