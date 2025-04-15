﻿using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface ITourStorageContract
{
    List<TourDataModel> GetList(DateTime? dateTime);

    TourDataModel? GetElementById(string id);

    TourDataModel? GetElementByName(string name);

    void AddElement(TourDataModel element);
}
