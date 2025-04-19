using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface IPlaceBusinessLogicContract
{
    List<PlaceDataModel> GetAllPlaces(string createrId);

    List<PlaceDataModel> GetAllPlacesByGroup(string createrId, string groupId);

    PlaceDataModel GetPlaceByData(string createrId, string data);

    void InsertPlace(PlaceDataModel model);

    void UpdatePlace(PlaceDataModel model);

    void DeletePlace(string createrId, string id);
}