using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IvanSusaninProject_DataBase.Implementations;

public class TripStorageContract : ITripStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;
    private readonly IMapper _mapper;

    public TripStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            // Конфигурация маппинга для TripGuide
            cfg.CreateMap<TripGuide, TripGuideDataModel>()
                .ConstructUsing(src => new TripGuideDataModel(src.TripId, src.GuideId));

            cfg.CreateMap<TripGuideDataModel, TripGuide>()
                .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.TripId))
                .ForMember(dest => dest.GuideId, opt => opt.MapFrom(src => src.GuideId))
                .ForMember(dest => dest.Trip, opt => opt.Ignore())
                .ForMember(dest => dest.Guide, opt => opt.Ignore());

            // Конфигурация маппинга для TripPlace
            cfg.CreateMap<TripPlace, TripPlaceDataModel>()
                .ConstructUsing(src => new TripPlaceDataModel(src.PlaceId, src.TripId));

            cfg.CreateMap<TripPlaceDataModel, TripPlace>()
                .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.TripId))
                .ForMember(dest => dest.PlaceId, opt => opt.MapFrom(src => src.PlaceId))
                .ForMember(dest => dest.Trip, opt => opt.Ignore())
                .ForMember(dest => dest.Place, opt => opt.Ignore());

            // Конфигурация маппинга для Trip
            cfg.CreateMap<Trip, TripDataModel>()
                .ForMember(dest => dest.TripPlaces, opt => opt.MapFrom(src => src.TripPlaces))
                .ForMember(dest => dest.TripGuides, opt => opt.MapFrom(src => src.TripGuides));

            cfg.CreateMap<TripDataModel, Trip>()
                .ForMember(dest => dest.TripPlaces, opt => opt.Ignore())
                .ForMember(dest => dest.TripGuides, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()))
                .ForMember(dest => dest.GuarandorId, opt => opt.MapFrom(src => src.GuarandorId))
                .ForMember(dest => dest.StartCity, opt => opt.MapFrom(src => src.StartCity))
                .ForMember(dest => dest.EndCity, opt => opt.MapFrom(src => src.EndCity))
                .ForMember(dest => dest.TripDate, opt => opt.MapFrom(src => src.TripDate))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration));
        });

        _mapper = config.CreateMapper();
    }

    public void AddElement(TripDataModel tripDataModel)
    {
        try
        {
            ValidateRelatedEntities(tripDataModel);

            var trip = _mapper.Map<Trip>(tripDataModel);
            _dbContext.Trips.Add(trip);

            AddTripRelations(trip.Id, tripDataModel.TripPlaces, tripDataModel.TripGuides);

            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public TripDataModel? GetElementById(string creatorId, string id)
    {
        try
        {
            var trip = GetTripWithIncludes()
                .FirstOrDefault(t => t.Id == id && t.GuarandorId == creatorId);

            return trip != null ? _mapper.Map<TripDataModel>(trip) : null;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<TripDataModel> GetList(string guarantorId, DateTime? fromDate = null, DateTime? toDate = null, DateTime? tripDate = null)
    {
        try
        {
            var query = GetTripWithIncludes()
                .Where(t => t.GuarandorId == guarantorId);

            if (tripDate != null)
                query = query.Where(t => t.TripDate == tripDate);

            if (fromDate != null && toDate != null)
                query = query.Where(t => t.TripDate >= fromDate && t.TripDate <= toDate);

            return _mapper.Map<List<TripDataModel>>(query.ToList());
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(TripDataModel tripDataModel)
    {
        try
        {
            var existingTrip = GetTripWithIncludes()
                .FirstOrDefault(t => t.Id == tripDataModel.Id && t.GuarandorId == tripDataModel.GuarandorId)
                ?? throw new ElementNotFoundException(tripDataModel.Id);

            ValidateRelatedEntities(tripDataModel);

            _mapper.Map(tripDataModel, existingTrip);
            _dbContext.TripPlaces.RemoveRange(existingTrip.TripPlaces);
            _dbContext.TripGuides.RemoveRange(existingTrip.TripGuides);

            AddTripRelations(existingTrip.Id, tripDataModel.TripPlaces, tripDataModel.TripGuides);

            _dbContext.SaveChanges();
        }
        catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    #region Private Methods

    private IQueryable<Trip> GetTripWithIncludes()
    {
        return _dbContext.Trips
            .Include(t => t.TripPlaces)
            .Include(t => t.TripGuides);
    }

    private void ValidateRelatedEntities(TripDataModel tripDataModel)
    {
        if (tripDataModel.TripPlaces != null)
        {
            var placeIds = tripDataModel.TripPlaces.Select(p => p.PlaceId).ToList();
            var existingPlaces = _dbContext.Places
                .Where(p => placeIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToList();

            foreach (var place in tripDataModel.TripPlaces)
            {
                if (!existingPlaces.Contains(place.PlaceId))
                {
                    throw new Exception($"Place with id {place.PlaceId} not found");
                }
            }
        }

        if (tripDataModel.TripGuides != null)
        {
            var guideIds = tripDataModel.TripGuides.Select(g => g.GuideId).ToList();
            var existingGuides = _dbContext.Guides
                .Where(g => guideIds.Contains(g.Id))
                .Select(g => g.Id)
                .ToList();

            foreach (var guide in tripDataModel.TripGuides)
            {
                if (!existingGuides.Contains(guide.GuideId))
                {
                    throw new Exception($"Guide with id {guide.GuideId} not found");
                }
            }
        }
    }

    private void AddTripRelations(
        string tripId,
        IEnumerable<TripPlaceDataModel>? tripPlaces,
        IEnumerable<TripGuideDataModel>? tripGuides)
    {
        if (tripPlaces != null)
        {
            foreach (var place in tripPlaces)
            {
                _dbContext.TripPlaces.Add(new TripPlace
                {
                    TripId = tripId,
                    PlaceId = place.PlaceId
                });
            }
        }

        if (tripGuides != null)
        {
            foreach (var guide in tripGuides)
            {
                _dbContext.TripGuides.Add(new TripGuide
                {
                    TripId = tripId,
                    GuideId = guide.GuideId
                });
            }
        }
    }

    public bool CheckPlaceExists(string placeId)
    {
        return _dbContext.Places.Any(p => p.Id == placeId);
    }

    #endregion
}