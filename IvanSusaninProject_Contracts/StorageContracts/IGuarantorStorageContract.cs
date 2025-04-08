
using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IGuarantorStorageContract
{
    List<GuarantorDataModel> GetList();

    GuarantorDataModel? GetElementById(string id);

    GuarantorDataModel? GetElementByLogin(string login);

    void AddElement(GuarantorDataModel guarantorDataModel);
}