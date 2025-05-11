using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace IvanSusaninProject_DataBase.Implementations;

public class PlaceStorageContract : IPlaceStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public PlaceStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Place, PlaceDataModel>();
            cfg.CreateMap<PlaceDataModel, Place>();
        });
        _mapper = new Mapper(config);
    }

    public void AddElement(PlaceDataModel placeDataModel)
    {
        try
        {
            _dbContext.Places.Add(_mapper.Map<Place>(placeDataModel));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelElement(string creatorId, string id)
    {
        try
        {
            var element = GetPlaceById(id, creatorId) ?? throw new ElementNotFoundException(id);
            _dbContext.Places.Remove(element);
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

    public PlaceDataModel? GetElementById(string creatorId, string id)
    {
        try
        {
            return _mapper.Map<PlaceDataModel>(GetPlaceById(id, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public PlaceDataModel? GetElementByName(string creatorId, string name)
    {
        try
        {
            return _mapper.Map<PlaceDataModel>(GetPlaceByName(name, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<PlaceDataModel> GetList(string guarantorId, string? groupId = null)
    {
        try
        {
            var query = _dbContext.Places.Include(x => x.TripPlaces).Where(x => x.GuaranderId == guarantorId).AsQueryable();
            if (groupId is not null)
            {
                query = query.Where(x => x.GroupId == groupId);
            }
            return [.. query.Select(x => _mapper.Map<PlaceDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(PlaceDataModel placeDataModel)
    {
        try
        {
            var element = GetPlaceById(placeDataModel.Id, placeDataModel.GuarandorId) ?? throw new
            ElementNotFoundException(placeDataModel.Id);
            _dbContext.Places.Update(_mapper.Map(placeDataModel, element));
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

    private Place? GetPlaceById(string id, string creatorId) => _dbContext.Places.Where(x => x.GuaranderId == creatorId).FirstOrDefault(x => x.Id == id);

    private Place? GetPlaceByName(string name, string creatorId) => _dbContext.Places.Where(x => x.GuaranderId == creatorId).FirstOrDefault(x => x.Name == name);
}
