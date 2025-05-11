using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using Microsoft.AspNetCore.Mvc;

namespace IvanSusaninProject.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class PlaceController(IPlaceAdapter adapter) : ControllerBase
{
    private readonly IPlaceAdapter _adapter = adapter;

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
    public IActionResult Register([FromBody] PlaceBindingModel model)
    {
        return _adapter.RegisterPlace(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult ChangeInfo([FromBody] PlaceBindingModel model)
    {
        return _adapter.ChangePlaceInfo(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string creatorId, string id)
    {
        return _adapter.RemovePlace(creatorId, id).GetResponse(Request, Response);
    }

    [HttpGet]
    public IActionResult GetGroupRecords(string creatorId, string groupId)
    {
        return _adapter.GetListByGroup(creatorId, groupId).GetResponse(Request, Response);
    }
}
