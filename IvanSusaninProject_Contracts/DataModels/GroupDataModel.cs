using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class GroupDataModel(string id, int humanAmount, HumanType humanType, string executorId)
{
    public string Id { get; private set; } = id;
    public int HumanAmount { get; private set; } = humanAmount;
    public HumanType HumanType { get; private set; } = humanType;
    public string ExecutorId { get; private set; } = executorId;
}
