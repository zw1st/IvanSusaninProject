
using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IGuideStrorageContract
{
    List<GuideDataModel> GetList();

    GuideDataModel? GetElementById(string id);

    GuideDataModel? GetElementByFIO(string fio);

    void AddElement(GuideDataModel guideDataModel);

    void UpdElement(GuideDataModel guideDataModel);

    void DelElement(string id);
}