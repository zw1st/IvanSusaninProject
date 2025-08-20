using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IExcursionStorageContract
{
    List<ExcursionDataModel> GetList(string creatorId, DateTime? dateTime, string? guideId);

    ExcursionDataModel? GetElementById(string creatorId, string id);

    ExcursionDataModel? GetElementByName(string creatorId, string name);

    void AddElement(ExcursionDataModel element);

    void UpdElement(ExcursionDataModel element);

    Task<List<ExcursionDataModel>> GetExcursionsByTourIds(string executorId, List<string> tripIds, CancellationToken ct);

    Task<List<object>> GetTripsWithDetailsByPeriod(DateTime startDate, DateTime endDate, string guaranderId, CancellationToken ct);
}
