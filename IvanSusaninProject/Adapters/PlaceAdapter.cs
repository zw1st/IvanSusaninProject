using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject.Adapters
{
    public class PlaceAdapter : IPlaceAdapter
    {
        private readonly IPlaceBusinessLogicContract _placeBusinessLogicContract;

        private readonly ILogger _logger;

        private readonly Mapper _mapper;

        public PlaceAdapter(IPlaceBusinessLogicContract placeBusinessLogicContract, ILogger logger)
        {
            _placeBusinessLogicContract = placeBusinessLogicContract;
            _logger = logger;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PlaceBindingModel, PlaceDataModel>();
                cfg.CreateMap<PlaceDataModel, PlaceViewModel>();
            });
            _mapper = new Mapper(config);
        }

        public PlaceOperationResponse ChangePlaceInfo(PlaceBindingModel model)
        {
            try
            {
                _placeBusinessLogicContract.UpdatePlace(_mapper.Map<PlaceDataModel>(model));
                return PlaceOperationResponse.NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException");
                return PlaceOperationResponse.BadRequest("Data is empty");
            }
            catch (MyValidationException ex)
            {
                _logger.LogError(ex, "MyValidationException");
                return PlaceOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
            }
            catch (ElementNotFoundException ex)
            {
                _logger.LogError(ex, "ElementNotFoundException");
                return PlaceOperationResponse.BadRequest($"Not found element by Id {model.Id} ");
            }
            catch (ElementExistsException ex)
            {
                _logger.LogError(ex, "ElementExistsException");
                return PlaceOperationResponse.BadRequest(ex.Message);
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return PlaceOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                PlaceOperationResponse.InternalServerError(ex.Message);
            }
        }

        public PlaceOperationResponse GetElement(string creatorId, string data)
        {
            try
            {
                return PlaceOperationResponse.OK(_mapper.Map<PlaceViewModel>(_placeBusinessLogicContract.GetPlaceByData(creatorId, data)));
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException");
                return PlaceOperationResponse.BadRequest("Data is empty");
            }
            catch (MyValidationException ex)
            {
                _logger.LogError(ex, "MyValidationException");
                return PlaceOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
            }
            catch (ElementNotFoundException ex)
            {
                _logger.LogError(ex, "ElementNotFoundException");
                return PlaceOperationResponse.NotFound($"Not found element by data {data} ");
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return PlaceOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                PlaceOperationResponse.InternalServerError(ex.Message);
            }
        }

        public PlaceOperationResponse GetList(string creatorId)
        {
            try
            {
                return PlaceOperationResponse.OK([.. _placeBusinessLogicContract.GetAllPlaces(creatorId).Select(x => _mapper.Map<PlaceViewModel>(x))]);
            }
            catch (NullListException)
            {
                _logger.LogError("NullListException");
                return PlaceOperationResponse.NotFound("The list is not initialized");
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return PlaceOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                PlaceOperationResponse.InternalServerError(ex.Message);
            }
        }

        public PlaceOperationResponse GetListByGroup(string creatorId, string groupId)
        {
            try
            {
                return PlaceOperationResponse.OK([.. _placeBusinessLogicContract.GetAllPlacesByGroup(creatorId, groupId).Select(x => _mapper.Map<PlaceViewModel>(x))]);
            }
            catch (MyValidationException ex)
            {
                _logger.LogError(ex, "MyValidationException");
                return PlaceOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
            }
            catch (NullListException)
            {
                _logger.LogError("NullListException");
                return PlaceOperationResponse.NotFound("The list is not initialized");
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return PlaceOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                PlaceOperationResponse.InternalServerError(ex.Message);
            }
        }

        public PlaceOperationResponse RegisterPlace(PlaceBindingModel model)
        {
            try
            {
                _placeBusinessLogicContract.InsertPlace(_mapper.Map<PlaceDataModel>(model));
                return PlaceOperationResponse.NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException");
                return PlaceOperationResponse.BadRequest("Data is empty");
            }
            catch (MyValidationException ex)
            {
                _logger.LogError(ex, "MyValidationException");
                return PlaceOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
            }
            catch (ElementExistsException ex)
            {
                _logger.LogError(ex, "ElementExistsException");
                return PlaceOperationResponse.BadRequest(ex.Message);
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return PlaceOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                PlaceOperationResponse.InternalServerError(ex.Message);
            }
        }

        public PlaceOperationResponse RemovePlace(string creatorId, string id)
        {
            try
            {
                _placeBusinessLogicContract.DeletePlace(creatorId, id);
                return PlaceOperationResponse.NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException");
                return PlaceOperationResponse.BadRequest("Id is empty");
            }
            catch (MyValidationException ex)
            {
                _logger.LogError(ex, "MyValidationException");
                return PlaceOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
            }
            catch (ElementNotFoundException ex)
            {
                _logger.LogError(ex, "ElementNotFoundException");
                return PlaceOperationResponse.BadRequest($"Not found element by id: {id} ");
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return PlaceOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                PlaceOperationResponse.InternalServerError(ex.Message);
            }
        }
    }
}
