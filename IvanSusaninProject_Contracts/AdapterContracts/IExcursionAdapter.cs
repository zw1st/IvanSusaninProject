using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IExcursionAdapter
{
    ExcursionOperationResponse GetList(string creatorId);

    ExcursionOperationResponse GetElement(string creatorId, string data);

    ExcursionOperationResponse RegisterExcursion(ExcursionBindingModel excursionModel);
}
