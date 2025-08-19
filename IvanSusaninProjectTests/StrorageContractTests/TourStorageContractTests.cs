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
        var list = _tourStorageContract.GetList(_executor.Id);
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(3));
        AssertElement(list.First(x => x.Id == tour.Id), tour);
    }

    [Test]
    public void Try_GetElementById_WhenHaveRecord_Test()
    {
        var tourId = Guid.NewGuid().ToString();
        var tour = InsertTourToDatabaseAndReturn(tourId, "test1", "city", DateTime.UtcNow, DateTime.UtcNow, _executor.Id,
        excursions: [(tourId, _excursion.Id)],
        groups: [(tourId, _group.Id)]);
        AssertElement(_tourStorageContract.GetElementById(_executor.Id, tour.Id), tour);
    }

    [Test]
    public void Try_GetElementByName_WhenHaveRecord_Test()
    {
        var tourId = Guid.NewGuid().ToString();
        var tour = InsertTourToDatabaseAndReturn(tourId, "test1", "city", DateTime.UtcNow, DateTime.UtcNow, _executor.Id,
        excursions: [(tourId, _excursion.Id)],
        groups: [(tourId, _group.Id)]);
        AssertElement(_tourStorageContract.GetElementByName(_executor.Id, tour.Name), tour);
    }

    [Test]
    public void Try_AddElement_Test()
    {
        var tourId = Guid.NewGuid().ToString();


        // Создаем и проверяем место
        //var excursion = InsertExcursionToDatabaseAndReturn(_executor.Id, DateTime.UtcNow, _guide.Id);

        //var guide = InsertGuideToDatabaseAndReturn(_guarantor.Id);

        // Создаем модель тура
        var tour = CreateModel(
            tourId,
            "name1",
            "city",
            DateTime.UtcNow,
            DateTime.UtcNow,
            _executor.Id,
            excursions: [new TourExcursionDataModel(tourId, _excursion.Id)],
            groups: [new TourGroupDataModel(tourId, _group.Id)]
        );

        // Сохраняем и проверяем
        _tourStorageContract.AddElement(tour);

        // Проверяем, что тур сохранился
        var tourFromDb = GetTourFromDatabase(tour.Id);
        Assert.That(tourFromDb, Is.Not.Null, "Тур не сохранилась в БД");

        // Проверяем связи
        Assert.That(
            IvanSusaninProject_DbContext.TourExcursions.Any(tp =>
                tp.TourId == tourId && tp.ExcursionId == _excursion.Id),
            Is.True, "Связь с екскурсией не создана");

        Assert.That(
            IvanSusaninProject_DbContext.TourGroups.Any(tg =>
                tg.TourId == tourId && tg.GroupId == _group.Id),
            Is.True, "Связь с группой не создана");

        AssertElement(tourFromDb, tour);
    }

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