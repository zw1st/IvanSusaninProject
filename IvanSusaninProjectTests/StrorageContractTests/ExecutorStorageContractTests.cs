using IvanSusaninProject_Contracts.DataModels;
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

internal class ExecutorStorageContractTests :BaseStorageContractTest
{
    private ExecutorStorageContract _executorStorageContract;

    [SetUp]
    public void SetUp()
    {
        _executorStorageContract = new ExecutorStorageContract(IvanSusaninProject_DbContext);
    }

    [TearDown]
    public void TearDown()
    {
        IvanSusaninProject_DbContext.Database.ExecuteSqlRaw("TRUNCATE \"Executors\" CASCADE; ");
    }

    [Test]
    public void Try_GetList_WhenHaveRecords_Test()
    {
        var executor =
        InsertExecutorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 1", "11111111");
        InsertExecutorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 2", "22222222");
        InsertExecutorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 3", "33333333");
        var list = _executorStorageContract.GetList();
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(3));
        AssertElement(list.First(), executor);
    }

    [Test]
    public void Try_GetElementById_WhenHaveRecord_Test()
    {
        var executor =
        InsertExecutorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 1", "11111111");
        AssertElement(_executorStorageContract.GetElementById(executor.Id), executor);
    }

    [Test]
    public void Try_GetElementByLogin_WhenHaveRecord_Test()
    {
        var executor =
        InsertExecutorToDatabaseAndReturn(Guid.NewGuid().ToString(), "fio 1", "11111111");
        AssertElement(_executorStorageContract.GetElementByLogin(executor.Login), executor);
    }

    [Test]
    public void Try_AddElement_Test()
    {
        var executor = CreateModel(Guid.NewGuid().ToString());
        _executorStorageContract.AddElement(executor);
        AssertElement(GetExecutorFromDatabase(executor.Id), executor);
    }

    private Executor InsertExecutorToDatabaseAndReturn(string id, string login /*= "test"*/, string password /*= "1111111"*/, string email = "example@example.com")
    {
        var executor = new Executor()
        {
            Id = id,
            Login = login,
            Password = password,
            Email = email
        };
        IvanSusaninProject_DbContext.Executors.Add(executor);
        IvanSusaninProject_DbContext.SaveChanges();
        return executor;
    }

    private static void AssertElement(ExecutorDataModel? actual, Executor expected)
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

    private static ExecutorDataModel CreateModel(string id, string login = "test", string password = "11111111", string email = "example@example.com")
    => new(id, login, password, email);

    private Executor? GetExecutorFromDatabase(string id) =>
    IvanSusaninProject_DbContext.Executors.Where(x => x.Id == id).FirstOrDefault();

    private static void AssertElement(Executor? actual, ExecutorDataModel expected)
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