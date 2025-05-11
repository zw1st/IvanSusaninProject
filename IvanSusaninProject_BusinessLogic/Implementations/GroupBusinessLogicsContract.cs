using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class GroupBusinessLogicsContract(IGroupStorageContract groupStorageContract, IPlaceStorageContract placeStorageContract, ILogger logger) : IGroupBusinessLogicsContract
{
    IGroupStorageContract _groupStorageContract = groupStorageContract;
    IPlaceStorageContract _placeStorageContract = placeStorageContract;

    private readonly ILogger _logger = logger;

    public void DeleteGroup(string creatorId, string id)
    {
        _logger.LogInformation("Delete by id: {id}", id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        if (!id.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        _groupStorageContract.DeleteElement(creatorId, id);
    }

    public List<GroupDataModel> GetAllGroups(string creatorId)
    {
        _logger.LogInformation("GetAllPosts params");
        return _groupStorageContract.GetList(creatorId) ?? throw new NullListException();
    }

    public GroupDataModel GetGroupById(string creatorId, string id)
    {
        _logger.LogInformation("Get element by id: {id}", id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        return _groupStorageContract.GetElementById(creatorId, id) ?? throw new ElementNotFoundException(id);
    }

    public void InsertGroup(GroupDataModel groupDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(groupDataModel));
        ArgumentNullException.ThrowIfNull(groupDataModel);
        _groupStorageContract.AddElement(groupDataModel);
    }

    public void LinkingGroupWithPlace(string creatorId, string groupId, string placeId)
    {
        _logger.LogInformation("GetAllPosts params");
        if (groupId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(groupId));
        }
        if (placeId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(placeId));
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(placeId));
        }
        var place = _placeStorageContract.GetElementById(creatorId, placeId) ?? throw new ElementNotFoundException(placeId);
        place.GroupId = groupId;
    }

    public void UpdateGroup(GroupDataModel groupDataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(groupDataModel));
        ArgumentNullException.ThrowIfNull(groupDataModel);
        _groupStorageContract.UpdateElement(groupDataModel);
    }
}

