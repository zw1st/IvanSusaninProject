using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_DataBase.Implementations;

internal class TourStorageContract : ITourStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;
    private readonly Mapper _mapper;

    public TourStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Tour, TourDataModel>();
            cfg.CreateMap<TourDataModel, Tour>();

            cfg.CreateMap<TourGroup, TourGroupDataModel>();
            cfg.CreateMap<TourGroupDataModel, TourGroup>();

            cfg.CreateMap<TourExcursion, TourExcursionDataModel>();
            cfg.CreateMap<TourExcursionDataModel, TourExcursion>();
        });
        _mapper = new Mapper(config);
    }


    public void AddElement(TourDataModel tourDataModel)
    {
        try
        {
            _dbContext.Tours.Add(_mapper.Map<Tour>(tourDataModel));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public TourDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<TourDataModel>(GetTourById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public TourDataModel? GetElementByName(string name)
    {
        try
        {
            return _mapper.Map<TourDataModel>(_dbContext.Tours.FirstOrDefault(x => x.Name == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<TourDataModel> GetList(string? executorId, DateTime? dateTime) 
    {
        try
        {
            var query = _dbContext.Tours.Include(x => x.TourGroups).Include(x => x.TourExcursions).AsQueryable();
            if (executorId is not null)
            {
                query = query.Where(x => x.ExecutorId == executorId);
            }
            if (dateTime is not null)
            {
                query = query.Where(x => x.StartDate <= dateTime && x.EndDate >= dateTime);
            }
            return [.. query.Select(x => _mapper.Map<TourDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Tour? GetTourById(string id) => _dbContext.Tours.FirstOrDefault(x => x.Id == id);

}
