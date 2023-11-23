using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{

    /// <summary>
    /// Interface
    /// </summary>
    public interface ICaregiverService
    {
        Task<ResponseMessage> GetAllCaregiverInitData(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllCaregiver(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllCaregiverList(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllCaregiverListByDepartmentID(RequestMessage requestMessage);
        Task<ResponseMessage> SaveCaregiver(RequestMessage requestMessage);
        Task<ResponseMessage> GetCaregiverById(RequestMessage requestMessage);
        Task<ResponseMessage> GetCaregiverForDropdown(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllCaregiverWithAllSearch(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllCaregiverGiverHistoryByDepartmentID(RequestMessage requestMessage);
        Task<ResponseMessage> RevertCaregiver(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllStatus(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllCaregiverGiverHistoryByCaregiverID(RequestMessage requestMessage);

    }
}
