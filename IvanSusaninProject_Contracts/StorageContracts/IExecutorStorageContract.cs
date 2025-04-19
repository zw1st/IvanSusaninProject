using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IExecutorStorageContract
{
    List<ExecutorDataModel> GetList();

    ExecutorDataModel? GetElementById(string id);

    ExecutorDataModel? GetElementByLogin(string name);

    void AddElement(ExecutorDataModel element);
}
