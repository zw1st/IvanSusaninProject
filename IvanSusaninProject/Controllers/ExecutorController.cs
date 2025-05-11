using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IvanSusaninProject.Controllers;


[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ExecutorController(IExecutorAdapter adapter) : ControllerBase
{
    private readonly IExecutorAdapter _adapter = adapter;

    [HttpGet]
    public IActionResult GetAllRecords()
    {
        return _adapter.GetList().GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string data)
    {
        return _adapter.GetElement(data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Register([FromBody] ExecutorBindingModel model)
    {
        return _adapter.RegisterExecutor(model).GetResponse(Request, Response);
    }

}
