using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_BusinessLogic.Implementations
{
    public class GuarantorBusinessLogicsContract(IGuarantorStorageContract guarantorStorageContract, ILogger logger) : IGuarantorBusinessLogicsContract
    {
        public List<GuarantorDataModel> GetAllGuarantors()
        {
            throw new NotImplementedException();
        }

        public GuarantorDataModel GetGuarantorByData(string data)
        {
            throw new NotImplementedException();
        }

        public void InsertGuarantor(GuarantorDataModel model)
        {
            throw new NotImplementedException();
        }
    }
}