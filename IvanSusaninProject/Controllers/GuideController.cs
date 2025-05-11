using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using Microsoft.AspNetCore.Mvc;

namespace IvanSusaninProject.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Produces("application/json")]
public class GuideController(IGuideAdapter adapter) : ControllerBase
{
    private readonly IGuideAdapter _adapter = adapter;

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
    public IActionResult Register([FromBody] GuideBindingModel model)
    {
        return _adapter.RegisterGuide(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult ChangeInfo([FromBody] GuideBindingModel model)
    {
        return _adapter.ChangeGuideInfo(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string creatorId, string id)
    {
        return _adapter.RemoveGuide(creatorId, id).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult LinkGuide(string creatorId, string guideId, string excursionId)
    {
        return _adapter.LinkGuideToExcursion(creatorId, guideId, excursionId).GetResponse(Request, Response);
    }
}