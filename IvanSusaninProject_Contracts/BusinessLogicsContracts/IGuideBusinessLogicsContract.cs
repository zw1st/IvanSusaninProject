using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface IGuideBusinessLogicsContract
{
    List<GuideDataModel> GetAllGuides(string createrId);

    GuideDataModel GetGuideByData(string createrId, string data);

    void InsertGuide(GuideDataModel model);

    void UpdateGuide(GuideDataModel model);

    void DeleteGuide(string createrId, string id);

    void LinkingGuideToExcursion(string excursionId);

}