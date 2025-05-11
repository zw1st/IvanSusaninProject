using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject.Adapters;

public class ExecutorAdapter : IExecutorAdapter
{
    private readonly IExecutorBusinessLogicsContract _executorBusinessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public ExecutorAdapter(IExecutorBusinessLogicsContract executorBusinessLogicContract, ILogger<ExecutorAdapter> logger)
    {
        _executorBusinessLogicContract = executorBusinessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ExecutorBindingModel, ExecutorDataModel>();
            cfg.CreateMap<ExecutorDataModel, ExecutorViewModel>();
        });
        _mapper = new Mapper(config);
    }


    public ExecutorOperationResponse GetElement(string data)
    {
        try
        {
            return ExecutorOperationResponse.OK(_mapper.Map<ExecutorViewModel>(_executorBusinessLogicContract.GetExecutorByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ExecutorOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ExecutorOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ExecutorOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ExecutorOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ExecutorOperationResponse GetList()
    {
        try
        {
            return ExecutorOperationResponse.OK([.. _executorBusinessLogicContract.GetAllExecutors().Select(x => _mapper.Map<ExecutorViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ExecutorOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ExecutorOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ExecutorOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ExecutorOperationResponse RegisterExecutor(ExecutorBindingModel executorModel)
    {
        try
        {
            _executorBusinessLogicContract.InsertExecutor(_mapper.Map<ExecutorDataModel>(executorModel));
            return ExecutorOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ExecutorOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ExecutorOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ExecutorOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ExecutorOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ExecutorOperationResponse.InternalServerError(ex.Message);
        }
    }
}
