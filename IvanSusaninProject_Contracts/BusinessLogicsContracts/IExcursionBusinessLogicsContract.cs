﻿using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface IExcursionBusinessLogicsContract
{
    List<ExcursionDataModel> GetAllExcursions(string creatorId, string? guideId);

    ExcursionDataModel GetExcursionByData(string creatorId, string data);

    void InsertExcursion(ExcursionDataModel excursionDataModel);
}