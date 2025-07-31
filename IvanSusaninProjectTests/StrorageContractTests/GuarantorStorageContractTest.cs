using IvanSusaninProject_Contracts.DataModels;
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
internal class GuarantorStorageContractTest : BaseStorageContractTest
{
    private GuarantorStorageContract _guarantorStorageContract;

    [SetUp]
    public void SetUp()
    {
        _guarantorStorageContract = new GuarantorStorageContract(IvanSusaninProject_DbContext);
    }

    [TearDown]
    public void TearDown()
    {
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Guarantors\" CASCADE; ");
    }

    [Test]
    public void Try_GetList_WhenHaveRecords_Test()
    {
        var guarantor =
        InsertGuarantorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 1", "11111111");
        InsertGuarantorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 2", "22222222");
        InsertGuarantorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 3", "33333333");
        var list = _guarantorStorageContract.GetList();
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(3));
        AssertElement(list.First(), guarantor);
    }

    [Test]
    public void Try_GetElementById_WhenHaveRecord_Test()
    {
        var guarantor =
        InsertGuarantorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 1", "11111111");
        AssertElement(_guarantorStorageContract.GetElementById(guarantor.Id), guarantor);
    }

    [Test]
    public void Try_GetElementByLogin_WhenHaveRecord_Test()
    {
        var guarantor =
        InsertGuarantorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 1", "11111111");
        AssertElement(_guarantorStorageContract.GetElementByLogin(guarantor.Login), guarantor);
    }

    [Test]
    public void Try_AddElement_Test()
    {
        var guarantor = CreateModel(Guid.NewGuid().ToString());
        _guarantorStorageContract.AddElement(guarantor);
        AssertElement(GetGuarantorFromDatabase(guarantor.Id), guarantor);
    }

    private Guarantor InsertGuarantorToDatabaseAndReturn(string id, string login /*= "test"*/, string password /*= "1111111"*/, string email = "example@example.com")
    {
        var guarantor = new Guarantor()
        {
            Id = id,
            Login = login,
            Password = password,
            Email = email
        };
        IvanSusaninProject_DbContext.Guarantors.Add(guarantor);
        IvanSusaninProject_DbContext.SaveChanges();
        return guarantor;
    }

    private static void AssertElement(GuarantorDataModel? actual, Guarantor expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Login, Is.EqualTo(expected.Login));
            Assert.That(actual.Password, Is.EqualTo(expected.Password));
            Assert.That(actual.Email, Is.EqualTo(expected.Email));
        });
    }

    private static GuarantorDataModel CreateModel(string id, string login = "test", string password = "11111111", string email = "example@example.com")
    => new(id, login, password, email);

    private Guarantor? GetGuarantorFromDatabase(string id) =>
    IvanSusaninProject_DbContext.Guarantors.Where(x => x.Id == id).FirstOrDefault();

    private static void AssertElement(Guarantor? actual, GuarantorDataModel expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Login, Is.EqualTo(expected.Login));
            Assert.That(actual.Password, Is.EqualTo(expected.Password));
            Assert.That(actual.Email, Is.EqualTo(expected.Email));
        });
    }
}