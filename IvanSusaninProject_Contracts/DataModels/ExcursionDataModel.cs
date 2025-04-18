using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class ExcursionDataModel(string id, string name, DateTime exursionDate, string guideId, string ExecutorId)
{
    public string Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public DateTime ExcursionDate { get; private set; } = exursionDate;
    public string GuideId {  get; private set; } = guideId;
    public string ExecutorId { get; private set; } = ExecutorId;
}
