using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class GuideBusinessLogicsContract(IGuideStrorageContract guideStrorageContract, IExcursionStorageContract excursionStorageContract, ILogger logger) : IGuideBusinessLogicsContract
{
    private readonly ILogger _logger = logger;

    private readonly IGuideStrorageContract _guideStorageContract = guideStrorageContract;

    private readonly IExcursionStorageContract _excursionStorageContract = excursionStorageContract;

    public void DeleteGuide(string creatorId, string id)
    {
        _logger.LogInformation("Delete by id: {creatorId} {id}", creatorId, id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        if (!id.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }
        _guideStorageContract.DelElement(creatorId, id);
    }

    public List<GuideDataModel> GetAllGuides(string creatorId)
    {
        _logger.LogInformation("GetAllGuides params: {creatorId}", creatorId);
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }
        return _guideStorageContract.GetList(creatorId) ?? throw new NullListException();
    }

    public GuideDataModel GetGuideByData(string creatorId, string data)
    {
        _logger.LogInformation("Get element by data: {creatorId}, {data}", creatorId, data);
        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (!data.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }
        return _guideStorageContract.GetElementById(creatorId, data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertGuide(GuideDataModel model)
    {
        _logger.LogInformation("New data: {json}",
        JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);
        _guideStorageContract.AddElement(model);
    }

    public void UpdateGuide(GuideDataModel model)
    {
        _logger.LogInformation("Update data: {json}",
        JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);
        _guideStorageContract.UpdElement(model);
    }

    public void LinkingGuideToExcursion(string creatorId, string guideId, string excursionId)
    {
        _logger.LogInformation("LinkingGuideToExcursion params: {createrId}, {guideId}, {excursionId}", creatorId, guideId, excursionId);
        if (excursionId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(excursionId));
        }
        if (!excursionId.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }
        if (guideId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(guideId));
        }
        if (!guideId.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }
        var excursion = _excursionStorageContract.GetElementById(creatorId, excursionId) ?? throw new ElementNotFoundException(excursionId);
        excursion.GuideId = guideId;
    }
}