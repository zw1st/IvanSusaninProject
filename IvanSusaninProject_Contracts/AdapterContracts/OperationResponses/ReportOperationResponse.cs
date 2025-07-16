using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class ReportOperationResponse : OperationResponse
{
    public static ReportOperationResponse OK(List<TripViewModel> data) => OK<ReportOperationResponse, List<TripViewModel>>(data);

    public static ReportOperationResponse OK(List<ExcursionViewModel> data) => OK<ReportOperationResponse, List<ExcursionViewModel>>(data);

    public static ReportOperationResponse OK(Stream data, string fileName) => OK<ReportOperationResponse, Stream>(data, fileName);

    public static ReportOperationResponse BadRequest(string message) => BadRequest<ReportOperationResponse>(message);

    public static ReportOperationResponse InternalServerError(string message) => InternalServerError<ReportOperationResponse>(message);
}
