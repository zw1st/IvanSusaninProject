using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
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
internal class GroupStorageContractTests : BaseStorageContractTest
{
    private GroupStorageContract _groupStorageContract;

    private Executor _executor;

    [SetUp]
    public void SetUp()
    {
        _groupStorageContract = new GroupStorageContract(IvanSusaninProject_DbContext);
        _executor = InsertExecutorToDatabaseAndReturn();
    }

    [TearDown]
    public void TearDown()
    {
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Groups\" CASCADE; ");
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Executors\" CASCADE; ");
    }

    [Test]
    public void Try_GetList_WhenHaveRecords_Test()
    {
        var group =
        InsertGroupToDatabaseAndReturn(2, HumanType.Youngs, _executor.Id);
        InsertGroupToDatabaseAndReturn(3, HumanType.Children, _executor.Id);
        InsertGroupToDatabaseAndReturn(4, HumanType.Teenagers, _executor.Id);
        var list = _groupStorageContract.GetList(_executor.Id);
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(3));
        AssertElement(list.FirstOrDefault(x => x.Id == group.Id), group);
    }

    [Test]
    public void Try_GetElementById_WhenHaveRecord_Test()
    {
        var group =
        InsertGroupToDatabaseAndReturn(2, HumanType.Youngs, _executor.Id);
        AssertElement(_groupStorageContract.GetElementById(_executor.Id, group.Id), group);
    }

    [Test]
    public void Try_UpdElement_Test()
    {
        var group = InsertGroupToDatabaseAndReturn(2, HumanType.Youngs, _executor.Id);
        var updgroup = CreateModel(group.Id, 3, HumanType.Teenagers, _executor.Id);
        _groupStorageContract.UpdateElement(updgroup);
        AssertElement(GetGroupFromDatabase(group.Id), updgroup);
    }

    [Test]
    public void Try_AddElement_Test()
    {
        var group = CreateModel(Guid.NewGuid().ToString(), 3, HumanType.Teenagers, _executor.Id);
        _groupStorageContract.AddElement(group);
        AssertElement(GetGroupFromDatabase(group.Id), group);
    }

    [Test]
    public void Try_DelElement_Test()
    {
        var group =
        InsertGroupToDatabaseAndReturn(2, HumanType.Youngs, _executor.Id);
        _groupStorageContract.DeleteElement(_executor.Id, group.Id);
        var element = GetGroupFromDatabase(group.Id);
        Assert.That(element, Is.Null);
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

    private Group InsertGroupToDatabaseAndReturn(int humanAmount, HumanType humanType, string executorId)
    {
        var group = new Group()
        {
            HumanAmount = humanAmount,
            ExecutorId = executorId,
            HumanType = humanType
        };
        IvanSusaninProject_DbContext.Groups.Add(group);
        IvanSusaninProject_DbContext.SaveChanges();
        return group;
    }

    private static void AssertElement(GroupDataModel? actual, Group expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.HumanAmount, Is.EqualTo(expected.HumanAmount));
            Assert.That(actual.HumanType, Is.EqualTo(expected.HumanType));
            Assert.That(actual.ExecutorId, Is.EqualTo(expected.ExecutorId));
        });
    }

    private static GroupDataModel CreateModel(string id, int humanAmount, HumanType humanType, string executorId)
    => new(id, humanAmount, humanType, executorId);

    private Executor? GetExecutorFromDatabase(string id) =>
    IvanSusaninProject_DbContext.Executors.Where(x => x.Id == id).FirstOrDefault();

    private static void AssertElement(Group? actual, GroupDataModel expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.HumanAmount, Is.EqualTo(expected.HumanAmount));
            Assert.That(actual.HumanType, Is.EqualTo(expected.HumanType));
            Assert.That(actual.ExecutorId, Is.EqualTo(expected.ExecutorId));
        });
    }

    private Group? GetGroupFromDatabase(string id) =>
    IvanSusaninProject_DbContext.Groups.Where(x => x.Id == id).FirstOrDefault();
}