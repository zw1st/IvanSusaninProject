﻿using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using Microsoft.EntityFrameworkCore;

namespace IvanSusaninProject_DataBase.Implementations;

public class GroupStorageContract : IGroupStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;
    private readonly Mapper _mapper;

    public GroupStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Group, GroupDataModel>();
            cfg.CreateMap<GroupDataModel, Group>();

            cfg.CreateMap<TourGroup, TourGroupDataModel>();
            cfg.CreateMap<TourGroupDataModel, TourGroup>();
        });
        _mapper = new Mapper(config);
    }

    public void AddElement(GroupDataModel componentDataModel)
    {
        try
        {
            _dbContext.Groups.Add(_mapper.Map<Group>(componentDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", componentDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public GroupDataModel? GetElementById(string creatorId, string id)
    {
        try
        {
            return _mapper.Map<GroupDataModel>(GetGroupById(id, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdateElement(GroupDataModel componentDataModel)
    {
        try
        {
            var element = GetGroupById(componentDataModel.Id, componentDataModel.ExecutorId) ?? throw new ElementNotFoundException(componentDataModel.Id);
            _dbContext.Groups.Update(_mapper.Map(componentDataModel, element));
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

    public void DeleteElement(string creatorId, string id)
    {
        try
        {
            var element = GetGroupById(id, creatorId) ?? throw new ElementNotFoundException(id);
            _dbContext.Groups.Remove(element);
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

    private Group? GetGroupById(string id, string creatorId) => _dbContext.Groups.Where(x => x.ExecutorId == creatorId).FirstOrDefault(x => x.Id == id);

    public List<GroupDataModel> GetList(string? executorId)
    {
        try
        {
            var query = _dbContext.Groups.Include(x => x.TourGroups).AsQueryable();
            if (executorId is not null)
            {
                query = query.Where(x => x.ExecutorId == executorId);
            }
            return [.. query.Select(x => _mapper.Map<GroupDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }
}
