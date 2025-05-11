using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class ExecutorBusinessLogicsContract(IExecutorStorageContract executorStorageContract, ILogger logger) : IExecutorBusinessLogicsContract
{
    IExecutorStorageContract _executorStorageContract = executorStorageContract;
    private readonly ILogger _logger = logger;

    public List<ExecutorDataModel> GetAllExecutors()
    {
        _logger.LogInformation("GetAllPosts params");
        return _executorStorageContract.GetList() ?? throw new NullListException();
    }

    public ExecutorDataModel GetExecutorByData(string data)
    {
        _logger.LogInformation("Get element by data: {data}", data);
        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (data.IsGuid())
        {
            return _executorStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        }
        return _executorStorageContract.GetElementByLogin(data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertExecutor(ExecutorDataModel executorDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(executorDataModel));
        ArgumentNullException.ThrowIfNull(executorDataModel);
        _executorStorageContract.AddElement(executorDataModel);
    }
}
