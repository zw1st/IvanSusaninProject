using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Models;

namespace IvanSusaninProject.Adapters;

public class ReportAdapter : IReportAdapter
{
    private readonly IReportContract _reportContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public ReportAdapter(IReportContract reportContract, ILogger logger)
    {
        _reportContract = reportContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ExcursionDataModel, ExcursionViewModel>();
            cfg.CreateMap<TripDataModel, TripViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public async Task<ReportOperationResponse> CreateExcelDocumentExcursionsByTripsAsync(List<string> tripIds, string executorId, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateExcelDocumentExcursionsByTrips(tripIds, executorId, ct),
            "excursions.xslx");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreateExcelDocumentTripsAsync(DateTime startDate, DateTime endDate, string guarantorId, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateExcelDocumentTripsDetailsByPeriod(startDate, endDate, guarantorId, ct),
            "trips.xslx");
        }
        catch (IncorrectDatesException ex)
        {
            _logger.LogError(ex, "IncorrectDatesException");
            return ReportOperationResponse.BadRequest($"Incorrect dates: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreateWordDocumentExcursionsByTripsAsync(List<string> tripIds, string executorId, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateWordDocumentExcursionsByTrips(tripIds, executorId, ct),
            "excurtions.docx");
        }
        catch (IncorrectDatesException ex)
        {
            _logger.LogError(ex, "IncorrectDatesException");
            return ReportOperationResponse.BadRequest($"Incorrect dates: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> CreateWordDocumentTripsAsync(DateTime startDate, DateTime endDate, string guarantorId, CancellationToken ct)
    {
        try
        {
            return SendStream(await _reportContract.CreateWordDocumentTripsDetailsByPeriod(startDate, endDate, guarantorId, ct),
            "trips.docx");
        }
        catch (IncorrectDatesException ex)
        {
            _logger.LogError(ex, "IncorrectDatesException");
            return ReportOperationResponse.BadRequest($"Incorrect dates: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> GetDataExcursionsByTripsAsync(List<string> tripIds, string executorId, CancellationToken ct)
    {
        try
        {
            return ReportOperationResponse.OK([.. (await _reportContract.GetExcursionsByTrips(tripIds, executorId, ct)).Select(x => _mapper.Map<ExcursionViewModel>(x))]);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public async Task<ReportOperationResponse> GetDataTripsDetailsAsync(DateTime startDate, DateTime endDate, string guarantorId, CancellationToken ct)
    {
        try
        {
            var tripsData = await _reportContract.GetTripsDetailsByPeriod(startDate, endDate, guarantorId, ct);
            var viewModels = tripsData.Select(x => _mapper.Map<TripViewModel>(x)).ToList();
            return ReportOperationResponse.OK(viewModels);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    private static ReportOperationResponse SendStream(Stream stream, string fileName)
    {
        stream.Position = 0;
        return ReportOperationResponse.OK(stream, fileName);
    }
}
