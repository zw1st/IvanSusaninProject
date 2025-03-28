using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.Enums;

[Flags]
public enum HumanType
{
    None = 0,
    Children = 1,
    Teenagers = 2,
    Youngs = 4,
    Adults = 8,
}
