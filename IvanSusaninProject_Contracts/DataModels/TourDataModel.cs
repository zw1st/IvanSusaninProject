namespace IvanSusaninProject_Contracts.DataModels;

public class TourDataModel(string id, string name, string city, DateTime startDate, DateTime endDate, string executorId, List<TourExcursionDataModel> tourExcursions, List<TourGroupDataModel> tourGroups)
{
    public string Id {  get; private set; } = id;

    public string Name { get; private set; } = name;

    public string City { get; private set; } = city;

    public DateTime StartDate { get; private set; } = startDate;

    public DateTime EndDate { get; private set; } = endDate;

    public string ExecutorId { get; private set; } = executorId;

    public List<TourExcursionDataModel> TourExcursions { get; private set; } = tourExcursions;

    public List<TourGroupDataModel> TourGroups { get; private set; } = tourGroups;
}