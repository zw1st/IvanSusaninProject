using IvanSusaninProject_Contracts.DataModels;
using System.Collections.Generic;
using System.IO;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts
{
    public interface IReportContract
    {
        // Отчет со списком экскурсий по выбранным поездкам
        Task<List<ExcursionDataModel>> GetExcursionsByTrips(List<string> tripIds, string executorId, CancellationToken ct);

        Task<Stream> CreateWordDocumentExcursionsByTrips(List<string> tripIds, string executorId, CancellationToken ct);

        Task<Stream> CreateExcelDocumentExcursionsByTrips(List<string> tripIds, string executorId, CancellationToken ct);


        // Отчет со сведениями за период по поездкам
        Task<List<object>> GetTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId, CancellationToken ct);

        Task<Stream> CreateWordDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId, CancellationToken ct);

        Task<Stream> CreateExcelDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId, CancellationToken ct);

    }
}