
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.Infrastructure;

namespace IvanSusaninProject_Contracts.DataModels;

public class TripDataModel(string id, string startcity, string endcity, DateTime tripDate, int duration, string guarandorId, List<TripPlaceDataModel> tripPlaces, List<TripGuideDataModel> tripGuides) : IValidation
{
    public string Id { get; private set; } = id;

    public string StartCity { get; private set; } = startcity;

    public string EndCity { get; private set; } = endcity;

    public DateTime TripDate {  get; private set; } = tripDate;

    public int Duration { get; private set; } = duration;

    public string GuarandorId { get; private set; } = guarandorId;

    public List<TripPlaceDataModel> TripPlaces { get; private set; } = tripPlaces;

    public List<TripGuideDataModel> TripGuides { get; private set; } = tripGuides;

    public void IValidate()
    {
        if (!Id.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (StartCity.IsEmpty())
            throw new ValidationException("This field is empty");

        if (EndCity.IsEmpty())
            throw new ValidationException("This field is empty");

        if (Id.IsEmpty())
            throw new ValidationException("This field is empty");

        if (!GuarandorId.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (GuarandorId.IsEmpty())
            throw new ValidationException("This field is empty");

        if (Duration <= 0)
            throw new ValidationException("Field Duration is less than or equal to 0");

        if ((TripPlaces?.Count ?? 0) == 0)
            throw new ValidationException("The trip must include places");

        if ((TripGuides?.Count ?? 0) == 0)
            throw new ValidationException("The trip must include guides");
    }
}