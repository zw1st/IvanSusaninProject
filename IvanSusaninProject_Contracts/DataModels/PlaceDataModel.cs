
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.Infrastructure;

namespace IvanSusaninProject_Contracts.DataModels;

public class PlaceDataModel(string id, string address, string city, string name, string groupId, string guarantorId) : IValidation
{
    public string Id { get; private set; } = id;

    public string Address { get; private set; } = address;

    public string City { get; private set; } = city;

    public string Name { get; private set; } = name;

    public string GroupId { get; set; } = groupId;

    public string GuarantorId { get; private set; } = guarantorId;

    public void IValidate()
    {
        if (!GuarantorId.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (GuarantorId.IsEmpty())
            throw new ValidationException("This field is empty");

        if (!Id.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (Id.IsEmpty())
            throw new ValidationException("This field is empty");

        if (Address.IsEmpty())
            throw new ValidationException("This field is empty");

        if (City.IsEmpty())
            throw new ValidationException("This field is empty");

        if (Name.IsEmpty())
            throw new ValidationException("This field is empty");

        if (!GroupId.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (GroupId.IsEmpty())
            throw new ValidationException("This field is empty");
    }
}