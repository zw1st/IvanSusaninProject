using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using Microsoft.AspNetCore.Mvc;

namespace IvanSusaninProject.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class TripController(ITripAdapter adapter) : ControllerBase
{
    private readonly ITripAdapter _adapter = adapter;

    [HttpGet]
    public IActionResult GetRecords(string creatorId)
    {
        return _adapter.GetList(creatorId).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetRecord(string creatorId, string data)
    {
        return _adapter.GetElement(creatorId, data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Register([FromBody] TripBindingModel model)
    {
        return _adapter.RegisterTrip(model).GetResponse(Request, Response);
    }

    [HttpGet]
    public IActionResult GetDateRecords(string creatorId, DateTime Date)
    {
        return _adapter.GetListByDate(creatorId, Date).GetResponse(Request, Response);
    }

    [HttpGet]
    public IActionResult GetByPeriodRecords(string creatorId, DateTime fromDate, DateTime toDate)
    {
        return _adapter.GetListByPeriod(creatorId, fromDate, toDate).GetResponse(Request, Response);
    }
}