using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class ExcursionDataModel(string id, string name, DateTime excursionDate, string ExecutorId, string? guideId = null) : IValidation
{
    public string Id { get; private set; } = id;

    public string Name { get; private set; } = name;

    public DateTime ExcursionDate { get; private set; } = excursionDate;

    public string ExecutorId { get; private set; } = ExecutorId;

    public string? GuideId { get; set; } = guideId;

    public void IValidate()
    {
        if (!Id.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (Id.IsEmpty())
            throw new ValidationException("This field is empty");

        if (Name.IsEmpty())
            throw new ValidationException("This field is empty");

        if (!ExecutorId.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (ExecutorId.IsEmpty())
            throw new ValidationException("This field is empty");

        if (!GuideId.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (GuideId.IsEmpty())
            throw new ValidationException("This field is empty");
    }
}
