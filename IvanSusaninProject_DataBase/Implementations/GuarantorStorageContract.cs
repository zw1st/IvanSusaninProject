using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Models;

namespace IvanSusaninProject_DataBase.Implementations;

internal class GuarantorStorageContract : IGuarantorStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public GuarantorStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration( cfg =>
        {
            cfg.CreateMap<Guarantor, GuarantorDataModel>();
            cfg.CreateMap<GuarantorDataModel, Guarantor>();
        });
        _mapper = new Mapper(config);
    }

    public void AddElement(GuarantorDataModel guarantorDataModel)
    {
        try
        {
            _dbContext.Guarantors.Add(_mapper.Map<Guarantor>(guarantorDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name ==
        "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", guarantorDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public GuarantorDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<GuarantorDataModel>(GetGuarantorById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public GuarantorDataModel? GetElementByLogin(string login)
    {
        try
        {
            return _mapper.Map<GuarantorDataModel>(GetGuarantorByLogin(login));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<GuarantorDataModel> GetList()
    {
        try
        {
            var query = _dbContext.Guarantors.AsQueryable();
            return [.. query.Select(x => _mapper.Map<GuarantorDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Guarantor? GetGuarantorById(string id) => _dbContext.Guarantors.FirstOrDefault(x => x.Id == id);

    private Guarantor? GetGuarantorByLogin(string login) => _dbContext.Guarantors.FirstOrDefault(x => x.Login == login);
}