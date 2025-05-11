using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject.Adapters;

public class GroupAdapter : IGroupAdapter
{
    private readonly IGroupBusinessLogicsContract _groupBusinessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public GroupAdapter(IGroupBusinessLogicsContract groupBusinessLogicContract, ILogger<GroupAdapter> logger)
    {
        _groupBusinessLogicContract = groupBusinessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GroupBindingModel, GroupDataModel>();
            cfg.CreateMap<GroupDataModel, GroupViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public GroupOperationResponse ChangeGroupInfo(GroupBindingModel groupModel)
    {
        try
        {
            _groupBusinessLogicContract.UpdateGroup(_mapper.Map<GroupDataModel>(groupModel));
            return GroupOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GroupOperationResponse.BadRequest("Data is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyMyValidationException");
            return GroupOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return GroupOperationResponse.BadRequest($"Not found element by Id {groupModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return GroupOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GroupOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return GroupOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GroupOperationResponse GetElement(string creatorId, string id)
    {
        try
        {
            return GroupOperationResponse.OK(_mapper.Map<GroupViewModel>(_groupBusinessLogicContract.GetGroupById(creatorId, id)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GroupOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return GroupOperationResponse.NotFound($"Not found element by id {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GroupOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return GroupOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GroupOperationResponse GetList(string creatorId)
    {
        try
        {
            return GroupOperationResponse.OK([.. _groupBusinessLogicContract.GetAllGroups(creatorId).Select(x => _mapper.Map<GroupViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return GroupOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GroupOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return GroupOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GroupOperationResponse LinkGroupWithPlace(string creatorId, string groupId, string placeId)
    {
        
        try
        {
            _groupBusinessLogicContract.LinkingGroupWithPlace(creatorId, groupId, placeId);
            return GroupOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GroupOperationResponse.BadRequest("Data is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return GroupOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return GroupOperationResponse.BadRequest($"Not found element by Id {groupId}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return GroupOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GroupOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return GroupOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GroupOperationResponse RegisterGroup(GroupBindingModel groupModel)
    {
        try
        {
            _groupBusinessLogicContract.InsertGroup(_mapper.Map<GroupDataModel>(groupModel));
            return GroupOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GroupOperationResponse.BadRequest("Data is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return GroupOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return GroupOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GroupOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return GroupOperationResponse.InternalServerError(ex.Message);
        }
    }

    public GroupOperationResponse RemoveGroup(string creatorId, string id)
    {
        try
        {
            _groupBusinessLogicContract.DeleteGroup(creatorId, id);
            return GroupOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return GroupOperationResponse.BadRequest("Id is empty");
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return GroupOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return GroupOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return GroupOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return GroupOperationResponse.InternalServerError(ex.Message);
        }
    }
}