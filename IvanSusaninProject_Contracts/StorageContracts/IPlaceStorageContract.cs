
using IvanSusaninProject_Contracts.DataModels;
using System.Text.RegularExpressions;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IPlaceStorageContract
{
    List<PlaceDataModel> GetList(string? groupId = null);

    PlaceDataModel? GetElementById(string id);

    PlaceDataModel? GetElementByName(string name);

    void AddElement(PlaceDataModel placeDataModel);

    void UpdElement(PlaceDataModel placeDataModel);

    void DelElement(string id);
}