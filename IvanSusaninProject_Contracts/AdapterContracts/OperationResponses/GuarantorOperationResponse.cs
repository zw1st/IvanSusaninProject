using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class GuarantorOperationResponse : OperationResponse
{
    public static GuarantorOperationResponse OK(List<GuarantorViewModel> data) => OK<GuarantorOperationResponse, List<GuarantorViewModel>>(data);

    public static GuarantorOperationResponse OK(GuarantorViewModel data) => OK<GuarantorOperationResponse, GuarantorViewModel>(data);

    public static GuarantorOperationResponse NoContent() => NoContent<GuarantorOperationResponse>();

    public static GuarantorOperationResponse NotFound(string message) => NotFound<GuarantorOperationResponse>(message);

    public static GuarantorOperationResponse BadRequest(string message) => BadRequest<GuarantorOperationResponse>(message);

    public static GuarantorOperationResponse InternalServerError(string message) => InternalServerError<GuarantorOperationResponse>(message);
}