
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.Infrastructure;

namespace IvanSusaninProject_Contracts.DataModels;

public class GuideDataModel(string id, string fio, int experience, int age, string guarandorId) : IValidation
{
    public string Id { get; private set; } = id;

    public string Fio { get; private set;} = fio;

    public int Experience { get; private set;} = experience;

    public int Age { get; private set;} = age;

    public string GuarandorId { get; private set;} = guarandorId;

    public void IValidate()
    {
        if (!GuarandorId.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (GuarandorId.IsEmpty())
            throw new ValidationException("This field is empty");

        if (!Id.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (Id.IsEmpty())
            throw new ValidationException("This field is empty");
        
        if (Fio.IsEmpty())
            throw new ValidationException("This field is empty");

        if (Experience <= 0)
            throw new ValidationException("Field Experience is less than or equal to 0");

        if (Age < 18)
            throw new ValidationException("Field Age is less than 18");
    }
}