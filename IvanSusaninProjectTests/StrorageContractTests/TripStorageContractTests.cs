using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Implementations;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace IvanSusaninProjectTests.StrorageContractTests;
[TestFixture]
internal class TripStorageContractTests : BaseStorageContractTest
{
    private TripStorageContract _tripStorageContract;

    private Guarantor _guarantor;
    private Place _place;
    private Guide _guide;
    private Group _group;
    private Executor _executor;

    [SetUp]
    public void SetUp()
    {
        _tripStorageContract = new TripStorageContract(IvanSusaninProject_DbContext);
        _guarantor = InsertGuarantorToDatabaseAndReturn();
        _executor = InsertExecutorToDatabaseAndReturn();
        _group = InsertGroupToDatabaseAndReturn(_executor.Id);
        _place = InsertPlaceToDatabaseAndReturn(_guarantor.Id, _group.Id);
        _guide = InsertGuideToDatabaseAndReturn(_guarantor.Id);
    }

    [TearDown]
    public void TearDown()
    {
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Trips\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Guarantors\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Executors\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Places\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Guides\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Groups\" CASCADE; ");
    }

    [Test]
    public void Try_GetList_WhenHaveRecords_Test()
    {
        var tripId = Guid.NewGuid().ToString();
        var tripId1 = Guid.NewGuid().ToString();
        var tripId2 = Guid.NewGuid().ToString();
        var trip = InsertTripToDatabaseAndReturn(tripId, _guarantor.Id, "test 1", "test 2", DateTime.UtcNow, 1,
        places: [(tripId, _place.Id)],
        guides: [(tripId, _guide.Id)]);
        InsertTripToDatabaseAndReturn(tripId1, _guarantor.Id, "test 3", "test 4", DateTime.UtcNow, 1,
        places: [(tripId1, _place.Id)],
        guides: [(tripId1, _guide.Id)]);
        InsertTripToDatabaseAndReturn(tripId2, _guarantor.Id, "test 5", "test 6", DateTime.UtcNow, 1,
        places: [(tripId2, _place.Id)],
        guides: [(tripId2, _guide.Id)]);
        var list = _tripStorageContract.GetList(_guarantor.Id);
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(3));
        AssertElement(list.First(x => x.Id == trip.Id), trip);
    }

    [Test]
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

    private Guarantor InsertGuarantorToDatabaseAndReturn(string login = "test", string password = "11111111", string email = "example@example.com")
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

    private Place InsertPlaceToDatabaseAndReturn(
    string guarantorId,
    string groupId,
    string address = "test",
    string city = "test",
    string name = null)
    {
        // Генерируем уникальное имя, если не передано
        name ??= $"place_{Guid.NewGuid()}";

        var place = new Place()
        {
            Name = name,
            Address = address,
            City = city,
            GuarantorId = guarantorId,
            GroupId = groupId
        };


        IvanSusaninProject_DbContext.Places.Add(place);
        IvanSusaninProject_DbContext.SaveChanges();

        return place;
    }

    private Trip InsertTripToDatabaseAndReturn(string? id,
        string guarandorId, string startCity, string endCity, DateTime tripDate, int duration,
        List<(string tripId, string placeId)>? places = null,
        List<(string tripId, string guideId)>? guides = null) 
    {
        var trip = new Trip()
        {
            Id = id ?? Guid.NewGuid().ToString(),
            GuarandorId = guarandorId,
            StartCity = startCity,
            EndCity = endCity,
            Duration = duration,
            TripDate = tripDate,
            TripPlaces = new List<TripPlace>(), // Инициализация
            TripGuides = new List<TripGuide>()  // Инициализация
        };
        if (places is not null)
        {
            foreach (var (tripId, placeId) in places)
            {
                IvanSusaninProject_DbContext.TripPlaces.Add(
                    new TripPlace
                    {
                        TripId = tripId,
                        PlaceId = placeId,
                    }
                );
            }
        }
        if (guides is not null)
        {
            foreach (var (tripId, guideId) in guides)
            {
                IvanSusaninProject_DbContext.TripGuides.Add(
                    new TripGuide { TripId = tripId, GuideId = guideId }
                );
            }
        }
        IvanSusaninProject_DbContext.Trips.Add(trip);
        IvanSusaninProject_DbContext.SaveChanges();
        return trip;
    }

    private static void AssertElement(TripDataModel? actual, Trip expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.StartCity, Is.EqualTo(expected.StartCity));
            Assert.That(actual.EndCity, Is.EqualTo(expected.EndCity));
            Assert.That(actual.TripDate, Is.EqualTo(expected.TripDate));
            Assert.That(actual.Duration, Is.EqualTo(expected.Duration));
            Assert.That(actual.GuarandorId, Is.EqualTo(expected.GuarandorId));
        });
        if (expected.TripGuides is not null)
        {
            Assert.That(actual.TripGuides, Is.Not.Null);
            Assert.That(actual.TripGuides,
            Has.Count.EqualTo(expected.TripGuides.Count));
            for (int i = 0; i < actual.TripGuides.Count; ++i)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actual.TripGuides[i].TripId,
                    Is.EqualTo(expected.TripGuides[i].TripId));
                    Assert.That(actual.TripGuides[i].GuideId,
                    Is.EqualTo(expected.TripGuides[i].GuideId));
                });
            }
        }
        else
        {
            Assert.That(actual.TripGuides, Is.Null);
        }
        if (expected.TripPlaces is not null)
        {
            Assert.That(actual.TripPlaces, Is.Not.Null);
            Assert.That(actual.TripPlaces,
            Has.Count.EqualTo(expected.TripPlaces.Count));
            for (int i = 0; i < actual.TripPlaces.Count; ++i)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actual.TripPlaces[i].TripId,
                    Is.EqualTo(expected.TripPlaces[i].TripId));
                    Assert.That(actual.TripPlaces[i].PlaceId,
                    Is.EqualTo(expected.TripPlaces[i].PlaceId));
                });
            }
        }
        else
        {
            Assert.That(actual.TripPlaces, Is.Null);
        }
    }

    private static TripDataModel CreateModel(string id, string startcity, string endcity, DateTime tripDate, int duration, string guarandorId, List<TripPlaceDataModel>? tripPlaces = null, List<TripGuideDataModel>? tripGuides = null)
    {
        return new(id, startcity, endcity, tripDate, duration, guarandorId, tripPlaces ?? [], tripGuides ?? []);
    }

    private Trip? GetTripFromDatabase(string id) =>
    IvanSusaninProject_DbContext.Trips.Where(x => x.Id == id).FirstOrDefault();

    private static void AssertElement(Trip? actual, TripDataModel expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.StartCity, Is.EqualTo(expected.StartCity));
            Assert.That(actual.EndCity, Is.EqualTo(expected.EndCity));
            Assert.That(actual.TripDate, Is.EqualTo(expected.TripDate));
            Assert.That(actual.Duration, Is.EqualTo(expected.Duration));
            Assert.That(actual.GuarandorId, Is.EqualTo(expected.GuarandorId));
        });
        if (expected.TripGuides is not null)
        {
            Assert.That(actual.TripGuides, Is.Not.Null);
            Assert.That(actual.TripGuides,
            Has.Count.EqualTo(expected.TripGuides.Count));
            for (int i = 0; i < actual.TripGuides.Count; ++i)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actual.TripGuides[i].TripId,
                    Is.EqualTo(expected.TripGuides[i].TripId));
                    Assert.That(actual.TripGuides[i].GuideId,
                    Is.EqualTo(expected.TripGuides[i].GuideId));
                });
            }
        }
        else
        {
            Assert.That(actual.TripGuides, Is.Null);
        }
        if (expected.TripPlaces is not null)
        {
            Assert.That(actual.TripPlaces, Is.Not.Null);
            Assert.That(actual.TripPlaces,
            Has.Count.EqualTo(expected.TripPlaces.Count));
            for (int i = 0; i < actual.TripPlaces.Count; ++i)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(actual.TripPlaces[i].TripId,
                    Is.EqualTo(expected.TripPlaces[i].TripId));
                    Assert.That(actual.TripPlaces[i].PlaceId,
                    Is.EqualTo(expected.TripPlaces[i].PlaceId));
                });
            }
        }
        else
        {
            Assert.That(actual.TripPlaces, Is.Null);
        }
    }
}
