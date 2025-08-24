using IvanSusaninProject_BusinessLogic.Implementations;
using IvanSusaninProject_BusinessLogic.OfficePackage;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Implementations;
using IvanSusaninProject_DataBase.Models;
using IvanSusaninProjectTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IvanSusaninProjectTests.IvanSusaninBusinessLogicTests;

[TestFixture]
public class ReportContractFileCreationTests
{
    private const string TestOutputDirectory = "TestOutput";
    private IvanSusaninProject_DbContext _dbContext;
    private IExcursionStorageContract _excursionStorage;
    private ITripStorageContract _tripStorage;
    private ITourStorageContract _tourStorage;
    private BaseWordBuilder _wordBuilder;
    private BaseExcelBuilder _excelBuilder;
    private ReportContract _reportContract;

    string guarantorId = Guid.NewGuid().ToString();
    string executorId = Guid.NewGuid().ToString();
    string trip1Id = Guid.NewGuid().ToString();
    string trip2Id = Guid.NewGuid().ToString();
    string group1Id = Guid.NewGuid().ToString();
    string group2Id = Guid.NewGuid().ToString();
    string group3Id = Guid.NewGuid().ToString();
    string place1Id = Guid.NewGuid().ToString();
    string place2Id = Guid.NewGuid().ToString();
    string place3Id = Guid.NewGuid().ToString();
    string guide1Id = Guid.NewGuid().ToString();
    string guide2Id = Guid.NewGuid().ToString();
    string guide3Id = Guid.NewGuid().ToString();
    string excursion1Id = Guid.NewGuid().ToString();
    string excursion2Id = Guid.NewGuid().ToString();
    string excursion3Id = Guid.NewGuid().ToString();
    string tour1Id = Guid.NewGuid().ToString();
    string tour2Id = Guid.NewGuid().ToString();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Создаем тестовую базу данных в памяти
        _dbContext = new IvanSusaninProject_DbContext(new ConfigurationDatabaseTest());
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        // Создаем реальные хранилища
        _excursionStorage = new ExcursionStorageContract(_dbContext);
        _tripStorage = new TripStorageContract(_dbContext); // Предполагаем, что такой класс существует
        _tourStorage = new TourStorageContract(_dbContext);
        // Создаем реальные билдеры
        _wordBuilder = new OpenXmlWordBuilder();
        _excelBuilder = new OpenXmlExcelBuilder();

        _reportContract = new ReportContract(_excursionStorage, _tripStorage, _tourStorage, _wordBuilder, _excelBuilder);

        // Наполняем базу тестовыми данными
        SeedTestData();

        // Создаем директорию для тестовых файлов
        if (!Directory.Exists(TestOutputDirectory))
        {
            Directory.CreateDirectory(TestOutputDirectory);
        }
    }

    [SetUp]
    public void SetUp()
    {
        // Создаем новые билдеры для каждого теста
        _wordBuilder = new OpenXmlWordBuilder();
        _excelBuilder = new OpenXmlExcelBuilder();

        _reportContract = new ReportContract(_excursionStorage, _tripStorage, _tourStorage, _wordBuilder, _excelBuilder);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // Очищаем базу данных
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();

        // Очищаем тестовую директорию
        /*if (Directory.Exists(TestOutputDirectory))
        {
            Directory.Delete(TestOutputDirectory, true);
        }*/
    }

    private void SeedTestData()
    {
        // Очищаем базу перед заполнением
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        // Создаем гарантора и исполнителя
        var guarantor = new Guarantor
        {
            Id = guarantorId,
            Login = "Test Guarantor",
            Password = "1234567890",
            Email = "guarantor@test.com"
        };

        var executor = new Executor
        {
            Id = executorId,
            Login = "Test Executor",
            Password = "0987654321",
            Email = "executor@test.com"
        };

        // Создаем группы
        var group1 = new Group
        {
            Id = group1Id,
            HumanAmount = 15,
            HumanType = HumanType.Adults,
            ExecutorId = executorId
        };

        var group2 = new Group
        {
            Id = group2Id,
            HumanAmount = 8,
            HumanType = HumanType.Children,
            ExecutorId = executorId
        };

        var group3 = new Group
        {
            Id = group3Id,
            HumanAmount = 10,
            HumanType = HumanType.Teenagers,
            ExecutorId = executorId
        };

        // Создаем места
        var place1 = new Place
        {
            Id = place1Id,
            Address = "Champ de Mars, Paris",
            City = "Paris",
            Name = "Eiffel Tower",
            GroupId = group1.Id,
            GuarantorId = guarantorId
        };

        var place2 = new Place
        {
            Id = place2Id,
            Address = "Piazza del Colosseo, Rome",
            City = "Rome",
            Name = "Colosseum",
            GroupId = group3.Id,
            GuarantorId = guarantorId
        };

        var place3 = new Place
        {
            Id = place3Id,
            Address = "Mountain Road 123",
            City = "Alps",
            Name = "Alps Resort",
            GroupId = group3.Id,
            GuarantorId = guarantorId
        };

        // Создаем гидов
        var guide1 = new Guide
        {
            Id = guide1Id,
            Fio = "John Smith",
            Experience = 5,
            Age = 30,
            GuarandorId = guarantorId
        };

        var guide2 = new Guide
        {
            Id = guide2Id,
            Fio = "Maria Garcia",
            Experience = 3,
            Age = 25,
            GuarandorId = guarantorId
        };

        var guide3 = new Guide
        {
            Id = guide3Id,
            Fio = "Robert Johnson",
            Experience = 7,
            Age = 35,
            GuarandorId = guarantorId
        };

        // Создаем поездки
        var trip1 = new Trip
        {
            Id = trip1Id,
            StartCity = "Moscow",
            EndCity = "Paris",
            TripDate = new DateTime(2024, 6, 15).ToUniversalTime(),
            Duration = 10,
            GuarandorId = guarantorId
        };

        var trip2 = new Trip
        {
            Id = trip2Id,
            StartCity = "St Petersburg",
            EndCity = "Alps",
            TripDate = new DateTime(2024, 12, 20).ToUniversalTime(),
            Duration = 7,
            GuarandorId = guarantorId
        };

        // Создаем связи между поездками и местами
        var tripPlace1 = new TripPlace { TripId = trip1.Id, PlaceId = place1.Id };
        var tripPlace2 = new TripPlace { TripId = trip1.Id, PlaceId = place2.Id };
        var tripPlace3 = new TripPlace { TripId = trip2.Id, PlaceId = place3.Id };

        // Создаем связи между поездками и гидами
        var tripGuide1 = new TripGuide { TripId = trip1.Id, GuideId = guide1.Id };
        var tripGuide2 = new TripGuide { TripId = trip1.Id, GuideId = guide2.Id };
        var tripGuide3 = new TripGuide { TripId = trip2.Id, GuideId = guide3.Id };

        // Создаем экскурсии
        var excursion1 = new Excursion
        {
            Id = excursion1Id,
            Name = "City Tour",
            ExcursionDate = DateTime.UtcNow,
            ExecutorId = executorId,
            GuideId = guide1.Id
        };

        var excursion2 = new Excursion
        {
            Id = excursion2Id,
            Name = "Museum Visit",
            ExcursionDate = DateTime.UtcNow,
            ExecutorId = executorId,
            GuideId = guide2.Id
        };

        var excursion3 = new Excursion
        {
            Id = excursion3Id,
            Name = "Park Walk",
            ExcursionDate = DateTime.UtcNow,
            ExecutorId = executorId,
            GuideId = guide3.Id
        };

        var tour1 = new Tour 
        {
            Id = tour1Id,
            Name = "Chest 100",
            StartDate = new DateTime(2024, 6, 15).ToUniversalTime(),
            EndDate = new DateTime(2024, 6, 30).ToUniversalTime(),
            ExecutorId = executorId
        };

        var tour2 = new Tour
        {
            Id = tour2Id,
            Name = "Easter",
            StartDate = new DateTime(2024, 4, 15).ToUniversalTime(),
            EndDate = new DateTime(2024, 6, 22).ToUniversalTime(),
            ExecutorId = executorId
        };

        var tourGroup1 = new TourGroup { TourId = tour1.Id, GroupId = group2.Id };
        var tourGroup2 = new TourGroup { TourId = tour1.Id, GroupId = group1.Id };
        var tourGroup3 = new TourGroup { TourId = tour2.Id, GroupId = group3.Id };

        var tourExcursion1 = new TourExcursion { TourId = tour1.Id, ExcursionId = excursion1.Id };
        var tourExcursion2 = new TourExcursion { TourId = tour1.Id, ExcursionId = excursion2.Id };
        var tourExcursion3 = new TourExcursion { TourId = tour2.Id, ExcursionId = excursion3.Id };


        // Добавляем все сущности в контекст
        _dbContext.Executors.AddRange(executor);
        _dbContext.Guarantors.Add(guarantor);
        _dbContext.Groups.AddRange(group1, group2, group3);
        _dbContext.Places.AddRange(place1, place2, place3);
        _dbContext.Guides.AddRange(guide1, guide2, guide3);
        _dbContext.Trips.AddRange(trip1, trip2);
        _dbContext.TripPlaces.AddRange(tripPlace1, tripPlace2, tripPlace3);
        _dbContext.TripGuides.AddRange(tripGuide1, tripGuide2, tripGuide3);
        _dbContext.Excursions.AddRange(excursion1, excursion2, excursion3);
        _dbContext.Tours.AddRange(tour1, tour2);
        _dbContext.TourExcursions.AddRange(tourExcursion1, tourExcursion2, tourExcursion3);
        _dbContext.TourGroups.AddRange(tourGroup1, tourGroup2, tourGroup3);

        _dbContext.SaveChanges();
    }

    [Test]
    public async Task CreateWordDocumentExcursionsByTrips_ShouldCreateValidDocument()
    {
        // Arrange
        var tripIds = new List<string> { trip1Id, trip2Id };
        var fileName = Path.Combine(TestOutputDirectory, "ExcursionsByTrips.docx");

        // Act
        var stream = await _reportContract.CreateWordDocumentExcursionsByTrips(tripIds, CancellationToken.None);

        // Сохраняем файл для проверки
        await SaveStreamToFile(stream, fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);
        Assert.That(new FileInfo(fileName).Length, Is.GreaterThan(0));

        Console.WriteLine($"Word document created: {fileName}");
    }

    [Test]
    public async Task CreateWordDocumentTripsDetailsByPeriod_ShouldCreateValidDocument()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1).ToUniversalTime();
        var endDate = new DateTime(2024, 12, 31).ToUniversalTime();
        var fileName = Path.Combine(TestOutputDirectory, "TripsDetails.docx");

        // Act
        var stream = await _reportContract.CreateWordDocumentTripsDetailsByPeriod(startDate, endDate, CancellationToken.None);

        // Сохраняем файл для проверки
        await SaveStreamToFile(stream, fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);
        Assert.That(new FileInfo(fileName).Length, Is.GreaterThan(0));

        Console.WriteLine($"Word document created: {fileName}");
    }

    [Test]
    public async Task CreateExcelDocumentExcursionsByTrips_ShouldCreateValidDocument()
    {
        // Arrange
        var tripIds = new List<string> { trip1Id, trip2Id };
        var fileName = Path.Combine(TestOutputDirectory, "ExcursionsByTrips.xlsx");

        // Act
        var stream = await _reportContract.CreateExcelDocumentExcursionsByTrips(tripIds, CancellationToken.None);

        // Сохраняем файл для проверки
        await SaveStreamToFile(stream, fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);
        Assert.That(new FileInfo(fileName).Length, Is.GreaterThan(0));

        Console.WriteLine($"Excel document created: {fileName}");
    }

    [Test]
    public async Task CreateExcelDocumentTripsDetailsByPeriod_ShouldCreateValidDocument()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1).ToUniversalTime();
        var endDate = new DateTime(2024, 12, 31).ToUniversalTime();

        var fileName = Path.Combine(TestOutputDirectory, "TripsDetails.xlsx");

        // Act
        var stream = await _reportContract.CreateExcelDocumentTripsDetailsByPeriod(startDate, endDate, CancellationToken.None);

        // Сохраняем файл для проверки
        await SaveStreamToFile(stream, fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);
        Assert.That(new FileInfo(fileName).Length, Is.GreaterThan(0));

        Console.WriteLine($"Excel document created: {fileName}");
    }

    [Test]
    public async Task CreateExcelDocumentTripsDetailsByPeriod_NoData_ShouldThrowException()
    {
        // Arrange
        var startDate = new DateTime(2025, 1, 1).ToUniversalTime(); // Будущая дата, данных нет
        var endDate = new DateTime(2025, 12, 31).ToUniversalTime();

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reportContract.CreateExcelDocumentTripsDetailsByPeriod(
                startDate,
                endDate,
                CancellationToken.None));
    }

    [Test]
    public async Task GetTripsDetailsByPeriod_ValidData_ShouldReturnTripDetails()
    {
        // Arrange
        var teststartDate = new DateTime(2024, 1, 1).ToUniversalTime();
        var testendDate = new DateTime(2024, 12, 31).ToUniversalTime();
        

        // Act
        var result = await _reportContract.GetTripsDetailsByPeriod(teststartDate, testendDate, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
        Assert.That(result[0].Trip, Is.Not.Null);
        Assert.That(result[0].Places.Count, Is.GreaterThan(0));
        Assert.That(result[0].Guides.Count, Is.GreaterThan(0));
    }

    [Test]
    public void GetTripsDetailsByPeriod_StartDateAfterEndDate_ShouldThrowArgumentException()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _reportContract.GetTripsDetailsByPeriod(startDate, endDate, CancellationToken.None));
    }

    [Test]
    public async Task GetExcursionsByTrips_ValidData_ShouldReturnExcursions()
    {
        // Arrange
        var tripIds = new List<string> { trip1Id, trip2Id };

        // Act
        var result = await _reportContract.GetExcursionsByTrips(tripIds, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
        Assert.That(result.All(e => !string.IsNullOrEmpty(e.TripName)), Is.True);
    }

    [Test]
    public async Task CreateWordDocumentPlacesByTours_ShouldCreateValidDocument()
    {
        // Arrange
        var tourIds = new List<string> { tour1Id, tour2Id };
        var fileName = Path.Combine(TestOutputDirectory, "PlacesByTours.docx");

        // Act
        var stream = await _reportContract.CreateWordDocumentPlacesByTours(tourIds, CancellationToken.None);

        // Сохраняем файл для проверки
        await SaveStreamToFile(stream, fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);
        Assert.That(new FileInfo(fileName).Length, Is.GreaterThan(0));

        Console.WriteLine($"Word document created: {fileName}");
    }

    [Test]
    public async Task CreateWordDocumentToursDetailsByPeriod_ShouldCreateValidDocument()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1).ToUniversalTime();
        var endDate = new DateTime(2024, 12, 31).ToUniversalTime();
        var fileName = Path.Combine(TestOutputDirectory, "ToursDetails.docx");

        // Act
        var stream = await _reportContract.CreateWordDocumentToursDetailsByPeriod(startDate, endDate, CancellationToken.None);

        // Сохраняем файл для проверки
        await SaveStreamToFile(stream, fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);
        Assert.That(new FileInfo(fileName).Length, Is.GreaterThan(0));

        Console.WriteLine($"Word document created: {fileName}");
    }

    [Test]
    public async Task CreateExcelDocumentPlacesByTours_ShouldCreateValidDocument()
    {
        // Arrange
        var tourIds = new List<string> { tour1Id, tour2Id };
        var fileName = Path.Combine(TestOutputDirectory, "PlacesByTours.xlsx");

        // Act
        var stream = await _reportContract.CreateExcelDocumentPlacesByTours(tourIds, CancellationToken.None);

        // Сохраняем файл для проверки
        await SaveStreamToFile(stream, fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);
        Assert.That(new FileInfo(fileName).Length, Is.GreaterThan(0));

        Console.WriteLine($"Excel document created: {fileName}");
    }

    [Test]
    public async Task CreateExcelDocumentToursDetailsByPeriod_ShouldCreateValidDocument()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1).ToUniversalTime();
        var endDate = new DateTime(2024, 12, 31).ToUniversalTime();
        var fileName = Path.Combine(TestOutputDirectory, "ToursDetails.xlsx");

        // Act
        var stream = await _reportContract.CreateExcelDocumentToursDetailsByPeriod(startDate, endDate, CancellationToken.None);

        // Сохраняем файл для проверки
        await SaveStreamToFile(stream, fileName);

        // Assert
        Assert.That(File.Exists(fileName), Is.True);
        Assert.That(new FileInfo(fileName).Length, Is.GreaterThan(0));

        Console.WriteLine($"Excel document created: {fileName}");
    }

    [Test]
    public async Task CreateExcelDocumentToursDetailsByPeriod_NoData_ShouldThrowException()
    {
        // Arrange
        var startDate = new DateTime(2025, 1, 1).ToUniversalTime(); // Будущая дата, данных нет
        var endDate = new DateTime(2025, 12, 31).ToUniversalTime();

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reportContract.CreateExcelDocumentToursDetailsByPeriod(
                startDate,
                endDate,
                CancellationToken.None));
    }

    [Test]
    public async Task GetPlacesByTours_ValidData_ShouldReturnPlaces()
    {
        // Arrange
        var tourIds = new List<string> { tour1Id, tour2Id };

        // Act
        var result = await _reportContract.GetPlacesByTours(tourIds, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
        Assert.That(result.All(p => !string.IsNullOrEmpty(p.TourName)), Is.True);
    }

    [Test]
    public async Task GetToursDetailsByPeriod_ValidData_ShouldReturnTourDetails()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1).ToUniversalTime();
        var endDate = new DateTime(2024, 12, 31).ToUniversalTime();

        // Act
        var result = await _reportContract.GetToursDetailsByPeriod(startDate, endDate, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
        Assert.That(result[0].Tour, Is.Not.Null);
        Assert.That(result[0].Groups.Count, Is.GreaterThan(0));
        Assert.That(result[0].Excursions.Count, Is.GreaterThan(0));
    }

    [Test]
    public void GetToursDetailsByPeriod_StartDateAfterEndDate_ShouldThrowArgumentException()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _reportContract.GetToursDetailsByPeriod(startDate, endDate, CancellationToken.None));
    }

    private async Task SaveStreamToFile(Stream stream, string filePath)
    {

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }
        var path = Path.Combine(Directory.GetCurrentDirectory(), filePath);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        stream.Position = 0;
        using var fileStream = new FileStream(path, FileMode.OpenOrCreate);
        await stream.CopyToAsync(fileStream);
    }
}