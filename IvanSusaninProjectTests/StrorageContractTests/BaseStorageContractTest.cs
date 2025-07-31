using IvanSusaninProject_Database;
using IvanSusaninProjectTests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProjectTests.StrorageContractTests;

internal abstract class BaseStorageContractTest
{
    protected IvanSusaninProject_DbContext IvanSusaninProject_DbContext { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        IvanSusaninProject_DbContext = new IvanSusaninProject_DbContext(new
        ConfigurationDatabaseTest());
        IvanSusaninProject_DbContext.Database.EnsureDeleted();
        IvanSusaninProject_DbContext.Database.EnsureCreated();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        IvanSusaninProject_DbContext.Database.EnsureDeleted();
        IvanSusaninProject_DbContext.Dispose();
    }
}
