using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BuisnessLogicContracts;

public interface ITourBuisnessLogicContract
{
    List<TourDataModel> GetAllTours(string creatorId);

    TourDataModel GetTourById(string id);

    void InsertTour(TourDataModel tourDataModel);
}
