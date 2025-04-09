using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface ITripBusinessLogicContract
{
    List<TripDataModel> GetAllTrips();

    List<TripDataModel> GetAllTripsByPeriod(DateTime fromDate, DateTime toDate);
    
    List<TripDataModel> GetAllTripsByDate(DateTime tripDate);

    TripDataModel GetTripByData(string data);

    void InsertTrip(TripDataModel model);

    void UpdateTrip(TripDataModel model);
}