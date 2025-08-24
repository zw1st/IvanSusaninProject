using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class ExcursionWithGuideDto
{
    public ExcursionDataModel Excursion { get; set; } = null!;

    public GuideDataModel? Guide { get; set; }
}