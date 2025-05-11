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

public class GuarantorAdapter : IGuarantorAdapter
{
    private readonly IGuarantorBusinessLogicsContract _guarantorBusinessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public GuarantorAdapter(IGuarantorBusinessLogicsContract guarantorBusinessLogicContract, ILogger<GuarantorAdapter> logger)
    {
        _guarantorBusinessLogicContract = guarantorBusinessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GuarantorBindingModel, GuarantorDataModel>();
            cfg.CreateMap<GuarantorDataModel, GuarantorViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public GuarantorOperationResponse GetElement(string data)
    {
        try
        {
            return GuarantorOperationResponse.OK(_mapper.Map<GuarantorViewModel>(_guarantorBusinessLogicContract.GetGuarantorByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GuarantorOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return GuarantorOperationResponse.NotFound($"Not found element by data {data} ");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GuarantorOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            GuarantorOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GuarantorOperationResponse GetList()
    {
        try
        {
            return GuarantorOperationResponse.OK([.. _guarantorBusinessLogicContract.GetAllGuarantors().Select(x => _mapper.Map<GuarantorViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return GuarantorOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GuarantorOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            GuarantorOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GuarantorOperationResponse RegisterGuarantor(GuarantorBindingModel model)
    {
        try
        {
            _guarantorBusinessLogicContract.InsertGuarantor(_mapper.Map<GuarantorDataModel>(model));
            return GuarantorOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GuarantorOperationResponse.BadRequest("Data is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return GuarantorOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return GuarantorOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GuarantorOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            GuarantorOperationResponse.InternalServerError(ex.Message);
        }
    }
}
