using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class GuarantorBusinessLogicsContract(IGuarantorStorageContract guarantorStorageContract, ILogger logger) : IGuarantorBusinessLogicsContract
{
    private readonly ILogger _logger = logger;

    private readonly IGuarantorStorageContract _guarantorStorageContract = guarantorStorageContract;

    public List<GuarantorDataModel> GetAllGuarantors()
    {
        _logger.LogInformation("GetAllGuarantors");
        return _guarantorStorageContract.GetList() ?? throw new NullListException();
    }

    public GuarantorDataModel GetGuarantorByData(string data)
    {
        _logger.LogInformation("Get element by data: {data}", data);
        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (data.IsGuid())
        {
            return _guarantorStorageContract.GetElementById(data) ?? throw new
            ElementNotFoundException(data);
        }
        return _guarantorStorageContract.GetElementByLogin(data) ?? throw new
        ElementNotFoundException(data);
    }

    public void InsertGuarantor(GuarantorDataModel model)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);
        _guarantorStorageContract.AddElement(model);
    }
}