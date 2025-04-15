using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BuisnessLogicContracts;

public interface IExecutorBusinessLogicsContract
{
    List<ExecutorDataModel> GetAllExecutors();

    ExecutorDataModel GetExecutorByData(string data);

    void InsertExecutor(ExecutorDataModel executorDataModel);
}
