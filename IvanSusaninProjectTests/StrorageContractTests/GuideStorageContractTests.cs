using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.StorageContracts;
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
internal class GuideStorageContractTests : BaseStorageContractTest
{
    private GuideStrorageContract _guideStorageContract;

    [SetUp]
    public void SetUp()
    {
        _guideStorageContract = new GuideStrorageContract(IvanSusaninProject_DbContext);
    }

    [TearDown]
    public void TearDown()
    {
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Guides\" CASCADE; ");
    }

    [Test]
    public void Try_GetList_WhenHaveRecords_Test()
    {
        var guarandorId = Guid.NewGuid().ToString();
        var guide =
        InsertGuideToDatabaseAndReturn(Guid.NewGuid().ToString(), guarandorId, "fio 1", 1, 18);
        InsertGuideToDatabaseAndReturn(Guid.NewGuid().ToString(), guarandorId, "fio 2", 1, 18);
        InsertGuideToDatabaseAndReturn(Guid.NewGuid().ToString(), guarandorId, "fio 3", 1, 18);
        var list = _guideStorageContract.GetList(guarandorId);
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(3));
        AssertElement(list.First(x => x.Id == guide.Id), guide);
    }

    [Test]
    public void Try_GetElementById_WhenHaveRecord_Test()
    {
        string guarandorId = Guid.NewGuid().ToString();
        var guide =
        InsertGuideToDatabaseAndReturn(Guid.NewGuid().ToString(), guarandorId, "fio 1", 1, 18);
        AssertElement(_guideStorageContract.GetElementById(guarandorId, guide.Id), guide);
    }

    [Test]
    public void Try_GetElementByFio_WhenHaveRecord_Test()
    {
        string guarandorId = Guid.NewGuid().ToString();
        var guide =
        InsertGuideToDatabaseAndReturn(Guid.NewGuid().ToString(), guarandorId, "fio 1", 1, 18);
        AssertElement(_guideStorageContract.GetElementByFIO(guarandorId, guide.Fio), guide);
    }

    [Test]
    public void Try_AddElement_Test()
    {
        string guarandorId = Guid.NewGuid().ToString();
        var guide = CreateModel(Guid.NewGuid().ToString(), guarandorId);
        _guideStorageContract.AddElement(guide);
        AssertElement(GetGuideFromDatabase(guide.Id), guide);
    }

    [Test]
    public void Try_UpdElement_Test()
    {
        string guarandorId = Guid.NewGuid().ToString();
        var guide = CreateModel(Guid.NewGuid().ToString(), guarandorId);
        InsertGuideToDatabaseAndReturn(guide.Id, guarandorId, guide.Fio, guide.Experience, guide.Age);
        _guideStorageContract.UpdElement(guide);
        AssertElement(GetGuideFromDatabase(guide.Id), guide);
    }

    [Test]
    public void Try_DelElement_Test()
    {
        string guarandorId = Guid.NewGuid().ToString();
        var guide =
        InsertGuideToDatabaseAndReturn(Guid.NewGuid().ToString(), guarandorId, "fio 1", 1, 18);
        _guideStorageContract.DelElement(guarandorId, guide.Id);
        var element = GetGuideFromDatabase(guide.Id);
        Assert.That(element, Is.Null);
    }

    private Guide InsertGuideToDatabaseAndReturn(string id, string guarandorId, string fio, int experience, int age)
    {
        var guide = new Guide()
        {
            Id = id,
            Fio = fio,
            Experience = experience,
            Age = age,
            GuarandorId = guarandorId
        };
        IvanSusaninProject_DbContext.Guides.Add(guide);
        IvanSusaninProject_DbContext.SaveChanges();
        return guide;
    }

    private static void AssertElement(GuideDataModel? actual, Guide expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Fio, Is.EqualTo(expected.Fio));
            Assert.That(actual.Experience, Is.EqualTo(expected.Experience));
            Assert.That(actual.Age, Is.EqualTo(expected.Age));
            Assert.That(actual.GuarandorId, Is.EqualTo(expected.GuarandorId));
        });
    }

    private static GuideDataModel CreateModel(string id, string guarandorId, string fio = "test", int experience = 1, int age = 18)
    => new(id, fio, experience, age, guarandorId);

    private Guide? GetGuideFromDatabase(string id) =>
    IvanSusaninProject_DbContext.Guides.Where(x => x.Id == id).FirstOrDefault();

    private static void AssertElement(Guide? actual, GuideDataModel expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Fio, Is.EqualTo(expected.Fio));
            Assert.That(actual.Experience, Is.EqualTo(expected.Experience));
            Assert.That(actual.Age, Is.EqualTo(expected.Age));
            Assert.That(actual.GuarandorId, Is.EqualTo(expected.GuarandorId));
        });
    }
}
