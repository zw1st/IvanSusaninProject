using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IGuideAdapter
{
    GuideOperationResponse GetList(string creatorId);

    GuideOperationResponse GetElement(string creatorId, string data);

    GuideOperationResponse RegisterGuide(GuideBindingModel model);

    GuideOperationResponse ChangeGuideInfo(GuideBindingModel model);

    GuideOperationResponse RemoveGuide(string creatorId, string id);

    GuideOperationResponse LinkGuideToExcursion(string creatorId, string guideId, string excursionId);
}