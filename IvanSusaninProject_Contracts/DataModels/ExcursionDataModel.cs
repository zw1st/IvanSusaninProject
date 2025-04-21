using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class ExcursionDataModel(string id, string name, DateTime exursionDate, string ExecutorId, string? guideId = null)
{
    public string Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public DateTime ExursionDate { get; } = exursionDate;
    public DateTime ExcursionDate { get; private set; }
    public string ExecutorId { get; private set; } = ExecutorId;
    public string GuideId { get; set; } = guideId;
}
