using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using Microsoft.AspNetCore.Mvc;

namespace IvanSusaninProject.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class TourController(ITourAdapter adapter) : ControllerBase
{
    private readonly ITourAdapter _adapter = adapter;

    [HttpGet]
    public IActionResult GetAllRecords(string creatorId, DateTime dateTime)
    {
        return _adapter.GetList(creatorId, dateTime).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string creatorId, string data)
    {
        return _adapter.GetElement(creatorId, data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Register([FromBody] TourBindingModel model)
    {
        return _adapter.RegisterTour(model).GetResponse(Request, Response);
    }

}