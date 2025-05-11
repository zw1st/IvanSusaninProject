using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using IvanSusaninProject_DataBase.Models;
using System;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IvanSusaninProject.Adapters;

public class ExcursionAdapter : IExcursionAdapter
{
    private readonly IExcursionBusinessLogicsContract _excursionBusinessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;


    public ExcursionAdapter(IExcursionBusinessLogicsContract excursionBusinessLogicContract, ILogger<ExcursionAdapter> logger)
    {
        _excursionBusinessLogicContract = excursionBusinessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ExcursionBindingModel, ExcursionDataModel>();
            cfg.CreateMap<ExcursionDataModel, ExcursionViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public ExcursionOperationResponse GetElement(string creatorId, string data)
    {
        try
        {
            return ExcursionOperationResponse.OK(_mapper.Map<ExcursionViewModel>(_excursionBusinessLogicContract.GetExcursionByData(creatorId, data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ExcursionOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ExcursionOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ExcursionOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ExcursionOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ExcursionOperationResponse GetList(string creatorId, DateTime dateTime, string? guideId)
    {
        try
        {
            return ExcursionOperationResponse.OK([.. _excursionBusinessLogicContract.GetAllExcursions( creatorId, dateTime, guideId).Select(x => _mapper.Map<ExcursionViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ExcursionOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ExcursionOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ExcursionOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ExcursionOperationResponse RegisterExcursion(ExcursionBindingModel excursionModel)
    {
        try
        {
            _excursionBusinessLogicContract.InsertExcursion(_mapper.Map<ExcursionDataModel>(excursionModel));
            return ExcursionOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ExcursionOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ExcursionOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ExcursionOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ExcursionOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ExcursionOperationResponse.InternalServerError(ex.Message);
        }
    }
}
