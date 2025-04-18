
using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using North_Bridge_Contract.Exceptions;
using System.Xml.Linq;

namespace IvanSusaninProject_DataBase.Implementations;

internal class GuideStrorageContract : IGuideStrorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public GuideStrorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Guide, GuideDataModel>();
            cfg.CreateMap<GuideDataModel, Guide>();
        });
        _mapper = new Mapper(config);
    }

    public void AddElement(GuideDataModel guideDataModel)
    {
        try
        {
            _dbContext.Guides.Add(_mapper.Map<Guide>(guideDataModel));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelElement(string id)
    {
        try
        {
            var element = GetGuideById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Guides.Remove(element);
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

    public GuideDataModel? GetElementByFIO(string fio)
    {
        try
        {
            return _mapper.Map<GuideDataModel>(GetGuideByFIO(fio));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public GuideDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<GuideDataModel>(GetGuideById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<GuideDataModel> GetList(string guarantorId)
    {
        try
        {
            var query = _dbContext.Guides.Include(x => x.TripGuides).Where(x => x.GuaranderId == guarantorId).AsQueryable();
            return [.. query.Select(x => _mapper.Map<GuideDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(GuideDataModel guideDataModel)
    {
        try
        {
            var element = GetGuideById(guideDataModel.Id) ?? throw new ElementNotFoundException(guideDataModel.Id);
            _dbContext.Guides.Update(_mapper.Map(guideDataModel, element));
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

    private Guide? GetGuideById(string id) => _dbContext.Guides.FirstOrDefault(x => x.Id == id);

    private Guide? GetGuideByFIO(string fio) => _dbContext.Guides.FirstOrDefault(x => x.Fio == fio);
}