using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IExcursionStorageContract
{
    List<ExcursionDataModel> GetList(string? executorId, DateTime? dateTime, string? guideId);

    ExcursionDataModel? GetElementById(string id);

    ExcursionDataModel? GetElementByName(string name);

    void AddElement(ExcursionDataModel element);
}
