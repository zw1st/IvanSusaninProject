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

internal class ExecutorStorageContract : IExecutorStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;
    private readonly Mapper _mapper;

    public ExecutorStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Executor, ExecutorDataModel>();
            cfg.CreateMap<ExecutorDataModel, Executor>();
        });
        _mapper = new Mapper(config);
    }

    public void AddElement(ExecutorDataModel executorDataModel)
    {
        try
        {
            _dbContext.Executors.Add(_mapper.Map<Executor>(executorDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", executorDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ExecutorDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<ExecutorDataModel>(GetExecutorById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ExecutorDataModel? GetElementByLogin(string name)
    {
        try
        {
            return _mapper.Map<ExecutorDataModel>(GetExecutorByLogin(name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<ExecutorDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Executors.Select(x => _mapper.Map<ExecutorDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Executor? GetExecutorById(string id) => _dbContext.Executors.FirstOrDefault(x => x.Id == id);

    private Executor? GetExecutorByLogin(string login) => _dbContext.Executors.FirstOrDefault(x => x.Login == login);


}
