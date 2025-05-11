using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IPlaceAdapter
{
    PlaceOperationResponse GetList(string creatorId);

    PlaceOperationResponse GetListByGroup(string creatorId, string groupId);

    PlaceOperationResponse GetElement(string creatorId, string data);

    PlaceOperationResponse RegisterPlace(PlaceBindingModel model);

    PlaceOperationResponse ChangePlaceInfo(PlaceBindingModel model);

    PlaceOperationResponse RemovePlace(string creatorId, string id);
}