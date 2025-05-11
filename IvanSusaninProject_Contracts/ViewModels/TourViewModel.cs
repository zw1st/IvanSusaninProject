﻿using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ViewModels;

public class TourViewModel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string ExecutorId { get; set; }
    public required string ExecutorLogin { get; set; }
    public List<TourExcursionDataModel>? Excursions { get; set; }
    public List<TourGroupDataModel>? Groups { get; set; }
}
