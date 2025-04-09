
using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface IGuarantorBusinessLogicsContract
{
    List<GuarantorDataModel> GetAllGuarantors();

    GuarantorDataModel GetGuarantorByData(string data);

    void InsertGuarantor(GuarantorDataModel model);
}