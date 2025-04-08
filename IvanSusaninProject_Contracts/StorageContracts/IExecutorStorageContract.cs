using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IExecutorStorageContract
{
    List<ExecutorDataModel> GetList();

    ExecutorDataModel? GetElementById(string id);

    ExecutorDataModel? GetElementByLogin(string name);

    void AddElement(ExecutorDataModel element);
}
