using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class ExecutorOperationResponse : OperationResponse
{
    public static ExecutorOperationResponse OK(List<ExecutorViewModel> data) => OK<ExecutorOperationResponse, List<ExecutorViewModel>>(data);

    public static ExecutorOperationResponse OK(ExecutorViewModel data) => OK<ExecutorOperationResponse, ExecutorViewModel>(data);

    public static ExecutorOperationResponse NoContent() => NoContent<ExecutorOperationResponse>();

    public static ExecutorOperationResponse BadRequest(string message) => BadRequest<ExecutorOperationResponse>(message);

    public static ExecutorOperationResponse NotFound(string message) => NotFound<ExecutorOperationResponse>(message);

    public static ExecutorOperationResponse InternalServerError(string message) => InternalServerError<ExecutorOperationResponse>(message);
}