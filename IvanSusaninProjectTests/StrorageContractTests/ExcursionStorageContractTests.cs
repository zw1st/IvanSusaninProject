using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_Contracts.StorageContracts;
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
internal class ExcursionStorageContractTests : BaseStorageContractTest
{
    private ExcursionStorageContract _excursionStorageContract;
    
    private Executor _executor;

    private Guide _guide;

    [SetUp]
    public void SetUp()
    {
        _excursionStorageContract = new ExcursionStorageContract(IvanSusaninProject_DbContext);
        _executor = InsertExecutorToDatabaseAndReturn();
        _guide = InsertGuideToDatabaseAndReturn(_executor.Id);

    }

    [TearDown]
    public void TearDown()
    {
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Guides\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Excursions\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Executors\" CASCADE; ");
    }

    [Test]
    public void Try_GetList_WhenHaveRecords_Test()
    {
        var excursion =
        InsertExcursionToDatabaseAndReturn("test1", _executor.Id, DateTime.UtcNow, _guide.Id);
        InsertExcursionToDatabaseAndReturn("test2", _executor.Id, DateTime.UtcNow, _guide.Id);
        InsertExcursionToDatabaseAndReturn("test3", _executor.Id, DateTime.UtcNow, _guide.Id);
        var list = _excursionStorageContract.GetList(_executor.Id, DateTime.UtcNow, _guide.Id);
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(3));
        AssertElement(list.FirstOrDefault(x => x.Id == excursion.Id), excursion);
    }

    [Test]
    public void Try_GetElementById_WhenHaveRecord_Test()
    {
        var excursion =
        InsertExcursionToDatabaseAndReturn("test1", _executor.Id, DateTime.UtcNow, _guide.Id);
        AssertElement(_excursionStorageContract.GetElementById(_executor.Id, excursion.Id), excursion);
    }

    [Test]
    public void Try_GetElementByName_WhenHaveRecord_Test()
    {
        var excursion =
        InsertExcursionToDatabaseAndReturn("test1", _executor.Id, DateTime.UtcNow, _guide.Id);
        AssertElement(_excursionStorageContract.GetElementByName(_executor.Id, excursion.Name), excursion);
    }

    [Test]
    public void Try_AddElement_Test()
    {
        var excursion = CreateModel(Guid.NewGuid().ToString(), "test1", DateTime.UtcNow, _executor.Id, _guide.Id);
        _excursionStorageContract.AddElement(excursion);
        AssertElement(GetExcursionFromDatabase(excursion.Id), excursion);
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

    private Guide InsertGuideToDatabaseAndReturn(string guarandorId, string fio = "fio1", int experience = 1, int age = 18)
    {
        var guide = new Guide()
        {
            Fio = fio,
            Experience = experience,
            Age = age,
            GuarandorId = guarandorId
        };
        IvanSusaninProject_DbContext.Guides.Add(guide);
        IvanSusaninProject_DbContext.SaveChanges();
        return guide;
    }

    private Excursion InsertExcursionToDatabaseAndReturn(string name, string executorId, DateTime excursionDate, string guideId)
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

    private static void AssertElement(ExcursionDataModel? actual, Excursion expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.ExcursionDate, Is.EqualTo(expected.ExcursionDate));
            Assert.That(actual.ExecutorId, Is.EqualTo(expected.ExecutorId));
            Assert.That(actual.GuideId, Is.EqualTo(expected.GuideId));
        });
    }

    private static ExcursionDataModel CreateModel(string id, string name, DateTime excursionDate, string executorId, string guideId)
    => new(id, name, excursionDate, executorId, guideId);

    private Excursion? GetExcursionFromDatabase(string id) =>
    IvanSusaninProject_DbContext.Excursions.Where(x => x.Id == id).FirstOrDefault();

    private static void AssertElement(Excursion? actual, ExcursionDataModel expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.ExcursionDate, Is.EqualTo(expected.ExcursionDate));
            Assert.That(actual.ExecutorId, Is.EqualTo(expected.ExecutorId));
            Assert.That(actual.GuideId, Is.EqualTo(expected.GuideId));
        });
    }
}