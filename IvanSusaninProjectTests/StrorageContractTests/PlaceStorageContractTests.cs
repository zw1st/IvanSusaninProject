using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Implementations;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace IvanSusaninProjectTests.StrorageContractTests;
[TestFixture]
internal class PlaceStorageContractTests : BaseStorageContractTest
{
    private PlaceStorageContract _placeStorageContract;
    private Guarantor _guarantor;
    private Group _group;
    private Executor _executor;

    [SetUp]
    public void SetUp()
    {
        _placeStorageContract = new PlaceStorageContract(IvanSusaninProject_DbContext);
        _guarantor = InsertGuarantorToDatabaseAndReturn();
        _executor = InsertExecutorToDatabaseAndReturn();
        _group = InsertGroupToDatabaseAndReturn(_executor.Id);
    }

    [TearDown]
    public void TearDown()
    {
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Places\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Guarantors\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Executors\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Groups\" CASCADE; ");
    }

    [Test]
    public void Try_GetList_WhenHaveRecords_Test()
    {
        var place =
        InsertPlaceToDatabaseAndReturn(_guarantor.Id, "name 1", _group.Id);
        InsertPlaceToDatabaseAndReturn(_guarantor.Id, "name 2", _group.Id);
        InsertPlaceToDatabaseAndReturn(_guarantor.Id, "name 3", _group.Id);
        var list = _placeStorageContract.GetList(_guarantor.Id);
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(3));
        AssertElement(list.First(), place);
    }

    [Test]
    public void Try_GetElementById_WhenHaveRecord_Test()
    {
        var place = InsertPlaceToDatabaseAndReturn(_guarantor.Id, "name 1", _group.Id);
        AssertElement(_placeStorageContract.GetElementById(_guarantor.Id, place.Id), place);
    }

    [Test]
    public void Try_GetElementByName_WhenHaveRecord_Test()
    {
        var place = InsertPlaceToDatabaseAndReturn(_guarantor.Id, "name 1", _group.Id);
        AssertElement(_placeStorageContract.GetElementByName(_guarantor.Id, place.Name), place);
    }

    [Test]
    public void Try_AddElement_Test()
    {
        var place = CreateModel(Guid.NewGuid().ToString(), "test1", "test2", "name1", _group.Id, _guarantor.Id);
        _placeStorageContract.AddElement(place);
        AssertElement(GetPlaceFromDatabase(place.Id), place);
    }

    [Test]
    public void Try_UpdElement_Test()
    {
        var place = InsertPlaceToDatabaseAndReturn(_guarantor.Id, "name 1", _group.Id);
        var updplace = CreateModel(place.Id, "test1", "test2", "name 2", _group.Id, _guarantor.Id);
        _placeStorageContract.UpdElement(updplace);
        AssertElement(GetPlaceFromDatabase(place.Id), updplace);
    }

    [Test]
    public void Try_DelElement_Test()
    {
        var place = InsertPlaceToDatabaseAndReturn(_guarantor.Id, "name 1", _group.Id);
        _placeStorageContract.DelElement(_guarantor.Id, place.Id);
        var element = GetPlaceFromDatabase(place.Id);
        Assert.That(element, Is.Null);
    }

    private Executor InsertExecutorToDatabaseAndReturn(string login = "test1", string password = "1111111", string email = "example@example.com")
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

    private Place InsertPlaceToDatabaseAndReturn(string guarantorId, string name, string groupId, string address = "test1", string city = "test2")
    {
        var place = new Place()
        {
            Name = name,
            GuarantorId = guarantorId,
            Address = address,
            City = city,
            GroupId = groupId
        };
        IvanSusaninProject_DbContext.Places.Add(place);
        IvanSusaninProject_DbContext.SaveChanges();
        return place;
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

    private static void AssertElement(PlaceDataModel? actual, Place expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.Address, Is.EqualTo(expected.Address));
            Assert.That(actual.City, Is.EqualTo(expected.City));
            Assert.That(actual.GuarantorId, Is.EqualTo(expected.GuarantorId));
            Assert.That(actual.GroupId, Is.EqualTo(expected.GroupId));
        });
    }

    private static PlaceDataModel CreateModel(string id,string address, string city, string name, string groupId, string guarantorId)
    => new(id, address, city, name, groupId, guarantorId);

    private Place? GetPlaceFromDatabase(string id) =>
    IvanSusaninProject_DbContext.Places.Where(x => x.Id == id).FirstOrDefault();

    private static void AssertElement(Place? actual, PlaceDataModel expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.Address, Is.EqualTo(expected.Address));
            Assert.That(actual.City, Is.EqualTo(expected.City));
            Assert.That(actual.GuarantorId, Is.EqualTo(expected.GuarantorId));
            Assert.That(actual.GroupId, Is.EqualTo(expected.GroupId));
        });
    }
}
