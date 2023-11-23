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
    public interface ICaregiverHistoryService
    {
        Task<ResponseMessage> GetAllCaregiverHistory(RequestMessage requestMessage);
        Task<ResponseMessage> SaveCaregiverHistory(RequestMessage requestMessage);
        Task<ResponseMessage> GetCaregiverHistoryById(RequestMessage requestMessage);

    }
}
