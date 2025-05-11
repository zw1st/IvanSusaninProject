using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using Microsoft.AspNetCore.Mvc;

namespace IvanSusaninProject.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class GuarantorController(IGuarantorAdapter adapter) : ControllerBase
{
    private readonly IGuarantorAdapter _adapter = adapter;

    [HttpGet]
    public IActionResult GetRecords()
    {
        return _adapter.GetList().GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string data)
    {
        return _adapter.GetElement(data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Register([FromBody] GuarantorBindingModel model)
    {
        return _adapter.RegisterGuarantor(model).GetResponse(Request, Response);
    }
}