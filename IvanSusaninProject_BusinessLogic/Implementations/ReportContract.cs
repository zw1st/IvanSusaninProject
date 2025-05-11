using IvanSusaninProject_BusinessLogic.OfficePackage;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.StorageContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class ReportContract : IReportContract
{
    private readonly IExcursionStorageContract _excursionStorage;
    private readonly ITripStorageContract _tripStorage;
    private readonly BaseWordBuilder _baseWordBuilder;
    private readonly BaseExcelBuilder _baseExcelBuilder;

    public ReportContract(
        IExcursionStorageContract excursionStorage,
        ITripStorageContract tripStorage,
        BaseWordBuilder baseWordBuilder,
        BaseExcelBuilder baseExcelBuilder)
    {
        _excursionStorage = excursionStorage;
        _tripStorage = tripStorage;
        _baseWordBuilder = baseWordBuilder;
        _baseExcelBuilder = baseExcelBuilder;
    }

    public List<ExcursionDataModel> GetExcursionsByTrips(List<string> tripIds, string executorId)
    {
        return _excursionStorage.GetExcursionsByTourIds(executorId, tripIds);
    }

    public List<object> GetTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be later than end date");

        return _excursionStorage.GetTripsWithDetailsByPeriod(startDate, endDate, guarantorId);
    }

    public Stream CreateWordDocumentExcursionsByTrips(List<string> tripIds, string executorId)
    {
        var data = GetExcursionsByTrips(tripIds, executorId) ??
                  throw new InvalidOperationException("No data found");

        var groupedExcursions = data
            .GroupBy(a => a.Name)
            .Select(g => new { TripName = g.Key, Excursions = g.Select(e => e.Name) });

        var tableData = new List<string[]>
    {
        new[] { "Поездка", "Экскурсии" }
    };

        foreach (var group in groupedExcursions)
        {
            tableData.Add(new[] {
            group.TripName,
            string.Join(", ", group.Excursions)
        });
        }

        return _baseWordBuilder
            .AddHeader("Список экскурсий по поездкам:")
            .AddTable(new[] { 3000, 5000 }, tableData)
            .Build();
    }

    public Stream CreateWordDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId)
    {
        var data = GetTripsDetailsByPeriod(startDate, endDate, guarantorId)
            .Cast<dynamic>()
            .ToList();

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableData = new List<string[]>
    {
        new[] { "Поездка", "Экскурсионная группа", "Гиды" }
    };

        foreach (var tripDetail in data)
        {
            var groups = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Places)
                    .Select(p => p.Group?.Name as string)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var guides = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Guides)
                    .Select(g => g.Fio as string));

            tableData.Add(new[]
            {
            tripDetail.Trip.Name as string ?? string.Empty,
            groups,
            guides
        });
        }

        return _baseWordBuilder
            .AddHeader($"Обзор поездок за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}")
            .AddTable(new[] { 3000, 3000, 3000 }, tableData)
            .Build();
    }

    public Stream CreateExcelDocumentExcursionsByTrips(List<string> tripIds, string executorId)
    {
        var data = GetExcursionsByTrips(tripIds, executorId) ??
                  throw new InvalidOperationException("No data found");

        var groupedExcursions = data
            .GroupBy(a => a.Name)
            .Select(g => new { TripName = g.Key, Excursions = g.Select(e => e.Name) });

        var tableRows = new List<string[]>
    {
        new[] { "Поездка", "Экскурсия" }
    };

        foreach (var group in groupedExcursions)
        {
            foreach (var excursion in group.Excursions)
            {
                tableRows.Add(new[] { group.TripName, excursion });
            }
        }

        return _baseExcelBuilder
            .AddHeader("Список экскурсий по поездкам", 0, 2)
            .AddParagraph("", 0)
            .AddTable(new[] { 20, 40 }, tableRows)
            .Build();
    }

    public Stream CreateExcelDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId)
    {
        var data = GetTripsDetailsByPeriod(startDate, endDate, guarantorId)
            .Cast<dynamic>()
            .ToList();

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableRows = new List<string[]>
    {
        new[] { "Поездка", "Экскурсионная группа", "Гиды" }
    };

        foreach (var tripDetail in data)
        {
            var groups = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Places)
                    .Select(p => p.Group?.Name as string)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var guides = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Guides)
                    .Select(g => g.Fio as string));

            tableRows.Add(new[]
            {
            tripDetail.Trip.Name as string ?? string.Empty,
            groups,
            guides
        });
        }

        return _baseExcelBuilder
            .AddHeader($"Обзор поездок за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}", 0, 3)
            .AddParagraph("", 0)
            .AddTable(new[] { 20, 20, 20 }, tableRows)
            .Build();
    }
}