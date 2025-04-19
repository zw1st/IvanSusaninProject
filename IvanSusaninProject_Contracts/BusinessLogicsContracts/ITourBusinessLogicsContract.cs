using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface ITourBusinessLogicsContract
{
    List<TourDataModel> GetAllTours(string creatorId);

    TourDataModel GetTourById(string createrId, string id);

    void InsertTour(TourDataModel tourDataModel);
}
