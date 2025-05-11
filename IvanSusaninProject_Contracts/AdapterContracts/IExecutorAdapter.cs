using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IExecutorAdapter
{
    ExecutorOperationResponse GetList();

    ExecutorOperationResponse GetElement(string data);

    ExecutorOperationResponse RegisterExecutor(ExecutorBindingModel executorModel);
}
