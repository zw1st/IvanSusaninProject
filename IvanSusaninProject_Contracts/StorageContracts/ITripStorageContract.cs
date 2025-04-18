
using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface ITripStorageContract
{
    List<TripDataModel> GetList(string guarantorId, DateTime? tripDate = null);

    TripDataModel? GetElementById(string id);

    void AddElement(TripDataModel tripDataModel);

    void UpdElement(TripDataModel tripDataModel);
}