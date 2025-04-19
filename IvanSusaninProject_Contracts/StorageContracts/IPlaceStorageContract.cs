﻿using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IPlaceStorageContract
{
    List<PlaceDataModel> GetList(string creatorId, string? groupId = null);

    PlaceDataModel? GetElementById(string creatorId, string id);

    PlaceDataModel? GetElementByName(string creatorId, string name);

    void AddElement(PlaceDataModel placeDataModel);

    void UpdElement(PlaceDataModel placeDataModel);

    void DelElement(string creatorId, string id);
}