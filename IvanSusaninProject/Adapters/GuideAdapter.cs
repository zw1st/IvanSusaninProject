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

public class GuideAdapter : IGuideAdapter
{
    private readonly IGuideBusinessLogicsContract _guideBusinessLogicsContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public GuideAdapter(IGuideBusinessLogicsContract guideBusinessLogicsContract, ILogger<GuideAdapter> logger)
    {
        _guideBusinessLogicsContract = guideBusinessLogicsContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GuideBindingModel, GuideDataModel>();
            cfg.CreateMap<GuideDataModel, GuideViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public GuideOperationResponse ChangeGuideInfo(GuideBindingModel model)
    {
        try
        {
            _guideBusinessLogicsContract.UpdateGuide(_mapper.Map<GuideDataModel>(model));
            return GuideOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GuideOperationResponse.BadRequest("Data is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return GuideOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return GuideOperationResponse.BadRequest($"Not found element by Id {model.Id} ");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return GuideOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GuideOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            GuideOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GuideOperationResponse GetElement(string creatorId, string data)
    {
        try
        {
            return GuideOperationResponse.OK(_mapper.Map<GuideViewModel>(_guideBusinessLogicsContract.GetGuideByData(creatorId, data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GuideOperationResponse.BadRequest("Data is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return GuideOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return GuideOperationResponse.NotFound($"Not found element by data {data} ");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GuideOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            GuideOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GuideOperationResponse GetList(string creatorId)
    {
        try
        {
            return GuideOperationResponse.OK([.. _guideBusinessLogicsContract.GetAllGuides(creatorId).Select(x => _mapper.Map<GuideViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return GuideOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GuideOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            GuideOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GuideOperationResponse LinkGuideToExcursion(string creatorId, string guideId, string excursionId)
    {
        try
        {
            _guideBusinessLogicsContract.LinkingGuideToExcursion(creatorId, guideId, excursionId);
            return GuideOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GuideOperationResponse.BadRequest("Data is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return GuideOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return GuideOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GuideOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            GuideOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GuideOperationResponse RegisterGuide(GuideBindingModel model)
    {
        try
        {
            _guideBusinessLogicsContract.InsertGuide(_mapper.Map<GuideDataModel>(model));
            return GuideOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GuideOperationResponse.BadRequest("Data is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return GuideOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return GuideOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GuideOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            GuideOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GuideOperationResponse RemoveGuide(string creatorId, string id)
    {
        try
        {
            _guideBusinessLogicsContract.DeleteGuide(creatorId , id);
            return GuideOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GuideOperationResponse.BadRequest("Id is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return GuideOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return GuideOperationResponse.BadRequest($"Not found element by id: {id} ");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GuideOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return
            GuideOperationResponse.InternalServerError(ex.Message);
        }
    }
}