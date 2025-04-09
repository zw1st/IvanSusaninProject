using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BuisnessLogicContracts;

public interface IExcursionBusinessLogicsContract
{
    List<ExcursionDataModel> GetAllExcursions(string creatorId, string? guideId);

    ExcursionDataModel GetExcursionByData(string data);

    void InsertExcursion(ExcursionDataModel excursionDataModel);
}
