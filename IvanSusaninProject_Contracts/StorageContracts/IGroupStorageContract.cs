using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IGroupStorageContract
{
    List<GroupDataModel> GetList(string? executorId);

    GroupDataModel? GetElementById(string id);

    void AddElement(GroupDataModel element);

    void UpdateElement(GroupDataModel element);

    void DeleteElement(string id);
}
