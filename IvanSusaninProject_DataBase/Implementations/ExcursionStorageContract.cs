using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using Microsoft.EntityFrameworkCore;

namespace IvanSusaninProject_DataBase.Implementations;

internal class ExcursionStorageContract : IExcursionStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;
    private readonly Mapper _mapper;

    public ExcursionStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Excursion, ExcursionDataModel>();
            cfg.CreateMap<ExecutorDataModel, Excursion>();

            cfg.CreateMap<TourExcursion, TourExcursionDataModel>();
            cfg.CreateMap<TourExcursionDataModel, TourExcursion>();
        });
        _mapper = new Mapper(config);
    }

    public void AddElement(ExcursionDataModel excursionDataModel)
    {
        try
        {
            _dbContext.Excursions.Add(_mapper.Map<Excursion>(excursionDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", excursionDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ExcursionDataModel? GetElementById(string creatorId, string id)
    {
        try
        {
            return _mapper.Map<ExcursionDataModel>(GetExcursionById(id, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ExcursionDataModel? GetElementByName(string creatorId, string name)
    {
        try
        {
            return _mapper.Map<ExcursionDataModel>(GetExcursionByName(name, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<ExcursionDataModel> GetList(string executorId, DateTime? dateTime, string? guideId)
    {
        try
        {
            var query = _dbContext.Excursions.Where(x => x.ExecutorId == executorId).Include(x => x.TourExcursions).AsQueryable();
            
            if (guideId is not null)
            {
                query = query.Where(x => x.GuideId == guideId);
            }
            if (dateTime is not null)
            {
                query = query.Where(x => x.ExcursionDate == dateTime);
            }
            return [.. query.Select(x => _mapper.Map<ExcursionDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Excursion? GetExcursionById(string id, string creatorId) => _dbContext.Excursions.Where(x => x.ExecutorId == creatorId).FirstOrDefault(x => x.Id == id);

    private Excursion? GetExcursionByName(string name, string creatorId) => _dbContext.Excursions.Where(x => x.ExecutorId == creatorId).FirstOrDefault(x => x.Name == name);
}
