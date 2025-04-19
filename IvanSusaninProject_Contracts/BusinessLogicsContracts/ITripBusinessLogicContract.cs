using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface ITripBusinessLogicContract
{
    List<TripDataModel> GetAllTrips(string createrId);

    List<TripDataModel> GetAllTripsByPeriod(string createrId, DateTime fromDate, DateTime toDate);
    
    List<TripDataModel> GetAllTripsByDate(string createrId, DateTime tripDate);

    TripDataModel GetTripByData(string createrId, string data);

    void InsertTrip(TripDataModel model);

    void UpdateTrip(TripDataModel model);
}