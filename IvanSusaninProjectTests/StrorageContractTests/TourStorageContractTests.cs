using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Implementations;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProjectTests.StrorageContractTests;

[TestFixture]
internal class TourStorageContractTests : BaseStorageContractTest
{
    private TourStorageContract _tourStorageContract;

    private Executor _executor;
    private Group _group;
    private Excursion _excursion;
    private Guide _guide;
    private Guarantor _guarantor;

    [SetUp]
    public void SetUp()
    {
        _tourStorageContract = new TourStorageContract(IvanSusaninProject_DbContext);
        _guarantor = InsertGuarantorToDatabaseAndReturn();
        _executor = InsertExecutorToDatabaseAndReturn();
        _guide = InsertGuideToDatabaseAndReturn(_guarantor.Id);
        _group = InsertGroupToDatabaseAndReturn(_executor.Id);
        _excursion = InsertExcursionToDatabaseAndReturn(_executor.Id, DateTime.UtcNow, _guide.Id);
    }

    [TearDown]
    public void TearDown()
    {
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Tours\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Guarantors\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Executors\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Excursions\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Guides\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Groups\" CASCADE; ");
    }

    [Test]
    public void Try_GetList_WhenHaveRecords_Test()
    {
        var tourId = Guid.NewGuid().ToString();
        var tourId1 = Guid.NewGuid().ToString();
        var tourId2 = Guid.NewGuid().ToString();
        var tour = InsertTourToDatabaseAndReturn(tourId, "test1", "city", DateTime.UtcNow, DateTime.UtcNow, _executor.Id,
        excursions: [(tourId, _excursion.Id)],
        groups: [(tourId, _group.Id)]);
        InsertTourToDatabaseAndReturn(tourId1, "test2", "city1", DateTime.UtcNow, DateTime.UtcNow, _executor.Id,
        excursions: [(tourId1, _excursion.Id)],
        groups: [(tourId1, _group.Id)]);
        InsertTourToDatabaseAndReturn(tourId2, "test3", "city2", DateTime.UtcNow, DateTime.UtcNow, _executor.Id,
        excursions: [(tourId2, _excursion.Id)],
        groups: [(tourId2, _group.Id)]);
        var list = _tourStorageContract.GetList(_executor.Id, DateTime.UtcNow);
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(3));
        AssertElement(list.First(x => x.Id == tour.Id), tour);
    }

    /*[Test]
    public void Try_GetElementById_WhenHaveRecord_Test()
    {
        var tripId = Guid.NewGuid().ToString();
        var trip = InsertTripToDatabaseAndReturn(tripId, _guarantor.Id, "test 1", "test 2", DateTime.UtcNow, 1,
        places: [(tripId, _place.Id)],
        guides: [(tripId, _guide.Id)]);
        AssertElement(_tripStorageContract.GetElementById(_guarantor.Id, trip.Id), trip);
    }

    [Test]
    public void Try_AddElement_Test()
    {
        var tripId = Guid.NewGuid().ToString();


        // Создаем и проверяем место
        var place = InsertPlaceToDatabaseAndReturn(_guarantor.Id, _group.Id);

        // Явная проверка, что место доступно через основной контекст
        var placeExists = _tripStorageContract.CheckPlaceExists(place.Id);
        Assert.That(placeExists, Is.True, "Место должно существовать в БД");
        var guide = InsertGuideToDatabaseAndReturn(_guarantor.Id);

        // Создаем модель поездки
        var trip = CreateModel(
            tripId,
            "test1",
            "test2",
            DateTime.UtcNow,
            1,
            _guarantor.Id,
            tripPlaces: [new TripPlaceDataModel(tripId, place.Id)],
            tripGuides: [new TripGuideDataModel(tripId, guide.Id)]
        );

        // Сохраняем и проверяем
        _tripStorageContract.AddElement(trip);

        // Проверяем, что поездка сохранилась
        var tripFromDb = GetTripFromDatabase(trip.Id);
        Assert.That(tripFromDb, Is.Not.Null, "Поездка не сохранилась в БД");

        // Проверяем связи
        Assert.That(
            IvanSusaninProject_DbContext.TripPlaces.Any(tp =>
                tp.TripId == tripId && tp.PlaceId == place.Id),
            Is.True, "Связь с местом не создана");

        Assert.That(
            IvanSusaninProject_DbContext.TripGuides.Any(tg =>
                tg.TripId == tripId && tg.GuideId == guide.Id),
            Is.True, "Связь с гидом не создана");

        AssertElement(tripFromDb, trip);
    }

    [Test]
    public void Try_UpdElement_Test()
    {
        var tripId = Guid.NewGuid().ToString();

        // Создаем и добавляем место и гида в базу
        var place = InsertPlaceToDatabaseAndReturn(_guarantor.Id, _group.Id);
        var guide = InsertGuideToDatabaseAndReturn(_guarantor.Id);

        // Сначала создаем поездку с местом и гидом
        var trip = InsertTripToDatabaseAndReturn(
            tripId,
            _guarantor.Id,
            "test 3",
            "test 4",
            DateTime.UtcNow,
            2,
            places: [(tripId, place.Id)],
            guides: [(tripId, guide.Id)]
        );

        // Обновляем поездку
        var updatedTrip = CreateModel(
            tripId,
            "test1",
            "test2",
            DateTime.UtcNow,
            1,
            _guarantor.Id,
            tripPlaces: [new TripPlaceDataModel(tripId, place.Id)],
            tripGuides: [new TripGuideDataModel(tripId, guide.Id)]
        );

        _tripStorageContract.UpdElement(updatedTrip);
        AssertElement(GetTripFromDatabase(trip.Id), updatedTrip);
    }*/

    private Guarantor InsertGuarantorToDatabaseAndReturn(string login = "test1", string password = "11111118", string email = "example@example.com")
    {
        var guarantor = new Guarantor()
        {
            Id = Guid.NewGuid().ToString(),
            Login = login,
            Password = password,
            Email = email
        };
        IvanSusaninProject_DbContext.Guarantors.Add(guarantor);
        IvanSusaninProject_DbContext.SaveChanges();
        return guarantor;
    }

    private Executor InsertExecutorToDatabaseAndReturn(string login = "test", string password = "1111111", string email = "example@example.com")
    {
        var executor = new Executor()
        {
            Login = login,
            Password = password,
            Email = email
        };
        IvanSusaninProject_DbContext.Executors.Add(executor);
        IvanSusaninProject_DbContext.SaveChanges();
        return executor;
    }

    private Guide InsertGuideToDatabaseAndReturn(
    string guarandorId,
    string fio = null,
    int experience = 1,
    int age = 18)
    {
        // Генерируем уникальное имя, если не передано
        fio ??= $"guide_{Guid.NewGuid()}";

        var guide = new Guide()
        {
            Fio = fio,
            Experience = experience,
            Age = age,
            GuarandorId = guarandorId
        };

        IvanSusaninProject_DbContext.Guides.Add(guide);
        IvanSusaninProject_DbContext.SaveChanges();

        // Убедимся, что гид действительно сохранен
        var savedGuide = IvanSusaninProject_DbContext.Guides
            .FirstOrDefault(g => g.Id == guide.Id);

        if (savedGuide == null)
        {
            throw new InvalidOperationException("Гид не был сохранен в БД");
        }

        return savedGuide;
    }
    private Group InsertGroupToDatabaseAndReturn(string guarandorId, int humanAmount = 1, HumanType humanType = HumanType.Adults)
    {
        var group = new Group()
        {
            ExecutorId = guarandorId,
            HumanAmount = humanAmount,
            HumanType = humanType,
        };
        IvanSusaninProject_DbContext.Groups.Add(group);
        IvanSusaninProject_DbContext.SaveChanges();
        return group;
    }

    private Excursion InsertExcursionToDatabaseAndReturn(string executorId, DateTime excursionDate, string guideId, string name = "name")
    {
        var excursion = new Excursion()
        {
            Name = name,
            ExecutorId = executorId,
            ExcursionDate = excursionDate,
            GuideId = guideId
        };
        IvanSusaninProject_DbContext.Excursions.Add(excursion);
        IvanSusaninProject_DbContext.SaveChanges();
        return excursion;
    }

    private Tour InsertTourToDatabaseAndReturn(string id,
        string name, string city, DateTime startDate, DateTime endDate, string executorId,
        List<(string tourId, string excursionId)>? excursions = null,
        List<(string tourId, string groupId)>? groups = null)
    {
        var tour = new Tour()
        {
            Id = id,
            Name = name,
            City = city,
            StartDate = startDate,
            EndDate = endDate,
            ExecutorId = executorId,
            TourExcursions = new List<TourExcursion>(), // Инициализация
            TourGroups = new List<TourGroup>()  // Инициализация
        };
        if (excursions is not null)
        {
            foreach (var (tourId, excursionId) in excursions)
            {
                IvanSusaninProject_DbContext.TourExcursions.Add(
                    new TourExcursion
                    {
                        TourId = tourId,
                        ExcursionId = excursionId,
                    }
                );
            }
        }
        if (groups is not null)
        {
            foreach (var (tourId, groupId) in groups)
            {
                IvanSusaninProject_DbContext.TourGroups.Add(
                    new TourGroup { TourId = tourId, GroupId = groupId }
                );
            }
        }
        IvanSusaninProject_DbContext.Tours.Add(tour);
        IvanSusaninProject_DbContext.SaveChanges();
        return tour;
    }

    private static void AssertElement(TourDataModel? actual, Tour expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.StartDate, Is.EqualTo(expected.StartDate));
            Assert.That(actual.EndDate, Is.EqualTo(expected.EndDate));
            Assert.That(actual.ExecutorId, Is.EqualTo(expected.ExecutorId));
            Assert.That(actual.City, Is.EqualTo(expected.City));
        });
        if (expected.TourExcursions is not null)
        {
            Assert.That(actual.TourExcursions, Is.Not.Null);
            Assert.That(actual.TourExcursions,
            Has.Count.EqualTo(expected.TourExcursions.Count));
            for (int i = 0; i < actual.TourExcursions.Count; ++i)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actual.TourExcursions[i].TourId,
                    Is.EqualTo(expected.TourExcursions[i].TourId));
                    Assert.That(actual.TourExcursions[i].ExcursionId,
                    Is.EqualTo(expected.TourExcursions[i].ExcursionId));
                });
            }
        }
        else
        {
            Assert.That(actual.TourExcursions, Is.Null);
        }
        if (expected.TourGroups is not null)
        {
            Assert.That(actual.TourGroups, Is.Not.Null);
            Assert.That(actual.TourGroups,
            Has.Count.EqualTo(expected.TourGroups.Count));
            for (int i = 0; i < actual.TourGroups.Count; ++i)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actual.TourGroups[i].TourId,
                    Is.EqualTo(expected.TourGroups[i].TourId));
                    Assert.That(actual.TourGroups[i].GroupId,
                    Is.EqualTo(expected.TourGroups[i].GroupId));
                });
            }
        }
        else
        {
            Assert.That(actual.TourGroups, Is.Null);
        }
    }

    private static TourDataModel CreateModel(string id, string name, string city, DateTime startDate, DateTime endDate, string executorId, List<TourExcursionDataModel>? excursions = null, List<TourGroupDataModel>? groups = null)
    {
        return new(id, name, city, startDate, endDate, executorId, excursions ?? [], groups ?? []);
    }

    private Tour? GetTourFromDatabase(string id) =>
    IvanSusaninProject_DbContext.Tours.Where(x => x.Id == id).FirstOrDefault();

    private static void AssertElement(Tour? actual, TourDataModel expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.StartDate, Is.EqualTo(expected.StartDate));
            Assert.That(actual.EndDate, Is.EqualTo(expected.EndDate));
            Assert.That(actual.ExecutorId, Is.EqualTo(expected.ExecutorId));
            Assert.That(actual.City, Is.EqualTo(expected.City));
        });
        if (expected.TourExcursions is not null)
        {
            Assert.That(actual.TourExcursions, Is.Not.Null);
            Assert.That(actual.TourExcursions,
            Has.Count.EqualTo(expected.TourExcursions.Count));
            for (int i = 0; i < actual.TourExcursions.Count; ++i)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actual.TourExcursions[i].TourId,
                    Is.EqualTo(expected.TourExcursions[i].TourId));
                    Assert.That(actual.TourExcursions[i].ExcursionId,
                    Is.EqualTo(expected.TourExcursions[i].ExcursionId));
                });
            }
        }
        else
        {
            Assert.That(actual.TourExcursions, Is.Null);
        }
        if (expected.TourGroups is not null)
        {
            Assert.That(actual.TourGroups, Is.Not.Null);
            Assert.That(actual.TourGroups,
            Has.Count.EqualTo(expected.TourGroups.Count));
            for (int i = 0; i < actual.TourGroups.Count; ++i)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actual.TourGroups[i].TourId,
                    Is.EqualTo(expected.TourGroups[i].TourId));
                    Assert.That(actual.TourGroups[i].GroupId,
                    Is.EqualTo(expected.TourGroups[i].GroupId));
                });
            }
        }
        else
        {
            Assert.That(actual.TourGroups, Is.Null);
        }
    }
}