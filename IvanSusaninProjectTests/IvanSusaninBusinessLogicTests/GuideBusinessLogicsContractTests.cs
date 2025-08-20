using IvanSusaninProject_BusinessLogic.Implementations;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Implementations;
using IvanSusaninProjectTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IvanSusaninProjectTests.IvanSusaninBusinessLogicTests;

[TestFixture]
internal class GuideBusinessLogicsContractTests
{
    private GuideBusinessLogicsContract _guideBusinessLogicsContract;
    private IGuideStrorageContract _guideStorageContract;
    private IExcursionStorageContract _excursionStorageContract;
    private IExecutorStorageContract _executorStorageContract;
    private IvanSusaninProject_DbContext _dbContext;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _dbContext = new IvanSusaninProject_DbContext(new ConfigurationDatabaseTest());
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        // Инициализируем реальные контракты хранилища
        _guideStorageContract = new GuideStrorageContract(_dbContext);
        _excursionStorageContract = new ExcursionStorageContract(_dbContext);
        _executorStorageContract = new ExecutorStorageContract(_dbContext);

        var logger = new Mock<ILogger>().Object;
        _guideBusinessLogicsContract = new GuideBusinessLogicsContract(
            _guideStorageContract, _excursionStorageContract, logger);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TearDown]
    public void TearDown()
    {
        // Очищаем базу данных после каждого теста
        _dbContext.Executors.RemoveRange(_dbContext.Executors);
        _dbContext.Guides.RemoveRange(_dbContext.Guides);
        _dbContext.Excursions.RemoveRange(_dbContext.Excursions);
        _dbContext.SaveChanges();
    }

    [Test]
    public void LinkingGuideToExcursion_ValidData_ShouldLinkGuideToExcursion_Test()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        
        var guideId = Guid.NewGuid().ToString();
        var excursionId = Guid.NewGuid().ToString();

        var executor = new ExecutorDataModel(creatorId, "test1", "11111111", "example@example.com");
        _executorStorageContract.AddElement(executor);
        // Создаем гида
        var guide = new GuideDataModel(guideId, "Test Guide", 5, 30, creatorId);
        _guideStorageContract.AddElement(guide);

        // Создаем экскурсию без гида
        var excursion = new ExcursionDataModel(excursionId, "Test Excursion", DateTime.UtcNow, creatorId);
        _excursionStorageContract.AddElement(excursion);

        // Act
        _guideBusinessLogicsContract.LinkingGuideToExcursion(creatorId, guideId, excursionId);

        // Assert
        var updatedExcursion = _excursionStorageContract.GetElementById(creatorId, excursionId);
        Assert.That(updatedExcursion, Is.Not.Null);
        Assert.That(updatedExcursion.GuideId, Is.EqualTo(guideId));
    }

    [Test]
    public void LinkingGuideToExcursion_ExcursionNotFound_ShouldThrowElementNotFoundException_Test()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var guideId = Guid.NewGuid().ToString();
        var nonExistentExcursionId = Guid.NewGuid().ToString();

        // Создаем гида
        var guide = new GuideDataModel(guideId, "Test Guide", 5, 30, creatorId);
        _guideStorageContract.AddElement(guide);

        // Act & Assert
        Assert.That(() => _guideBusinessLogicsContract.LinkingGuideToExcursion(
            creatorId, guideId, nonExistentExcursionId),
            Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void LinkingGuideToExcursion_NullExcursionId_ShouldThrowArgumentNullException_Test()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var guideId = Guid.NewGuid().ToString();

        // Act & Assert
        Assert.That(() => _guideBusinessLogicsContract.LinkingGuideToExcursion(
            creatorId, guideId, null),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void LinkingGuideToExcursion_EmptyExcursionId_ShouldThrowArgumentNullException_Test()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var guideId = Guid.NewGuid().ToString();

        // Act & Assert
        Assert.That(() => _guideBusinessLogicsContract.LinkingGuideToExcursion(
            creatorId, guideId, string.Empty),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void LinkingGuideToExcursion_NullGuideId_ShouldThrowArgumentNullException_Test()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var excursionId = Guid.NewGuid().ToString();
        var executor = new ExecutorDataModel(creatorId, "test1", "11111111", "example@example.com");
        _executorStorageContract.AddElement(executor);
        // Создаем экскурсию
        var excursion = new ExcursionDataModel(excursionId, "Test Excursion", DateTime.UtcNow, creatorId);
        _excursionStorageContract.AddElement(excursion);

        // Act & Assert
        Assert.That(() => _guideBusinessLogicsContract.LinkingGuideToExcursion(
            creatorId, null, excursionId),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void LinkingGuideToExcursion_EmptyGuideId_ShouldThrowArgumentNullException_Test()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var excursionId = Guid.NewGuid().ToString();

        var executor = new ExecutorDataModel(creatorId, "test1", "11111111", "example@example.com");
        _executorStorageContract.AddElement(executor);

        // Создаем экскурсию
        var excursion = new ExcursionDataModel(excursionId, "Test Excursion", DateTime.UtcNow, creatorId);
        _excursionStorageContract.AddElement(excursion);

        // Act & Assert
        Assert.That(() => _guideBusinessLogicsContract.LinkingGuideToExcursion(
            creatorId, string.Empty, excursionId),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void LinkingGuideToExcursion_NullCreatorId_ShouldThrowArgumentNullException_Test()
    {
        // Arrange
        var guideId = Guid.NewGuid().ToString();
        var excursionId = Guid.NewGuid().ToString();

        // Act & Assert
        Assert.That(() => _guideBusinessLogicsContract.LinkingGuideToExcursion(
            null, guideId, excursionId),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void LinkingGuideToExcursion_EmptyCreatorId_ShouldThrowArgumentNullException_Test()
    {
        // Arrange
        var guideId = Guid.NewGuid().ToString();
        var excursionId = Guid.NewGuid().ToString();

        // Act & Assert
        Assert.That(() => _guideBusinessLogicsContract.LinkingGuideToExcursion(
            string.Empty, guideId, excursionId),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void LinkingGuideToExcursion_GuideNotFound_ShouldNotThrowExceptionButNotLink_Test()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var nonExistentGuideId = Guid.NewGuid().ToString();
        var excursionId = Guid.NewGuid().ToString();

        var executor = new ExecutorDataModel(creatorId, "test1", "11111111", "example@example.com");
        _executorStorageContract.AddElement(executor);
        // Создаем экскурсию
        var excursion = new ExcursionDataModel(excursionId, "Test Excursion", DateTime.UtcNow, creatorId);
        _excursionStorageContract.AddElement(excursion);

        // Act
        _guideBusinessLogicsContract.LinkingGuideToExcursion(creatorId, nonExistentGuideId, excursionId);

        // Assert - метод не должен бросать исключение, но и не должен изменять GuideId
        var updatedExcursion = _excursionStorageContract.GetElementById(creatorId, excursionId);
        Assert.That(updatedExcursion, Is.Not.Null);
        Assert.That(updatedExcursion.GuideId, Is.Null); // GuideId должен остаться null
    }

    [Test]
    public void LinkingGuideToExcursion_AlreadyLinkedGuide_ShouldUpdateGuideId_Test()
    {
        // Arrange
        var creatorId = Guid.NewGuid().ToString();
        var guideId1 = Guid.NewGuid().ToString();
        var guideId2 = Guid.NewGuid().ToString();
        var excursionId = Guid.NewGuid().ToString();

        var executor = new ExecutorDataModel(creatorId, "test1", "11111111", "example@example.com");
        _executorStorageContract.AddElement(executor);
        // Создаем двух гидов
        var guide1 = new GuideDataModel(guideId1, "Guide 1", 5, 30, creatorId);
        var guide2 = new GuideDataModel(guideId2, "Guide 2", 3, 25, creatorId);
        _guideStorageContract.AddElement(guide1);
        _guideStorageContract.AddElement(guide2);

        // Создаем экскурсию с первым гидом
        var excursion = new ExcursionDataModel(excursionId, "Test Excursion", DateTime.UtcNow, creatorId, guideId1);
        _excursionStorageContract.AddElement(excursion);

        // Act - связываем со вторым гидом
        _guideBusinessLogicsContract.LinkingGuideToExcursion(creatorId, guideId2, excursionId);

        // Assert - GuideId должен обновиться
        var updatedExcursion = _excursionStorageContract.GetElementById(creatorId, excursionId);
        Assert.That(updatedExcursion, Is.Not.Null);
        Assert.That(updatedExcursion.GuideId, Is.EqualTo(guideId2));
    }
}