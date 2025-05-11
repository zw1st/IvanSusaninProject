using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IGroupAdapter
{
    GroupOperationResponse GetList(string creatorId);

    GroupOperationResponse GetElement(string creatorId, string data);

    GroupOperationResponse RegisterGroup(GroupBindingModel groupModel);

    GroupOperationResponse ChangeGroupInfo(GroupBindingModel groupModel);

    GroupOperationResponse RemoveGroup(string creatorId, string id);

    GroupOperationResponse LinkGroupWithPlace(string creatorId, string groupId, string placeId);
}
