using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BuisnessLogicContracts;

public interface IGroupBuisnessLogicContract
{
    List<GroupDataModel> GetAllGroups(string creatorId);

    GroupDataModel GetGroupById(string id);

    void InsertGroup(GroupDataModel groupDataModel);

    void UpdateGroup(GroupDataModel groupDataModel);

    void DeleteGroup(string id);

    void LinkingGroupWithPlace(string placeId);
}
