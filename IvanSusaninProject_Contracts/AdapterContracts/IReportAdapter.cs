using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IReportAdapter
{
    Task<ReportOperationResponse> GetDataTripsDetailsAsync(DateTime startDate, DateTime endDate, string guarantorId, CancellationToken ct);

    Task<ReportOperationResponse> GetDataExcursionsByTripsAsync(List<string> tripIds, string executorId, CancellationToken ct);

    Task<ReportOperationResponse> CreateWordDocumentTripsAsync(DateTime startDate, DateTime endDate, string guarantorId, CancellationToken ct);

    Task<ReportOperationResponse> CreateWordDocumentExcursionsByTripsAsync(List<string> tripIds, string executorId, CancellationToken ct);

    Task<ReportOperationResponse> CreateExcelDocumentTripsAsync(DateTime startDate, DateTime endDate, string guarantorId, CancellationToken ct);

    Task<ReportOperationResponse> CreateExcelDocumentExcursionsByTripsAsync(List<string> tripIds, string executorId, CancellationToken ct);
}