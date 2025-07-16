using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class GroupDataModel(string id, int humanAmount, HumanType humanType, string executorId) : IValidation
{
    public string Id { get; private set; } = id;

    public int HumanAmount { get; private set; } = humanAmount;

    public HumanType HumanType { get; private set; } = humanType;

    public string ExecutorId { get; private set; } = executorId;

    public void IValidate()
    {
        if (!Id.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (Id.IsEmpty())
            throw new ValidationException("This field is empty");

        if (!ExecutorId.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (ExecutorId.IsEmpty())
            throw new ValidationException("This field is empty");

        if (HumanAmount <= 0)
            throw new ValidationException("Field HumanAmount is less than or equal to 0");
    }
}
