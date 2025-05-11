using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IGuarantorAdapter
{
    GuarantorOperationResponse GetList();

    GuarantorOperationResponse GetElement(string data);

    GuarantorOperationResponse RegisterGuarantor(GuarantorBindingModel model);
}