using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BindingModels;

public class GroupBindingModel
{
    public string? Id { get;  set; }
    public int HumanAmount { get;  set; }
    public HumanType HumanType { get; set; }
    public string? ExecutorId { get; set; }
}
