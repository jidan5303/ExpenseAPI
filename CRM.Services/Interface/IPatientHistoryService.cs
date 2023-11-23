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
    public interface IPatientHistoryService
    {
        Task<ResponseMessage> GetAllPatientHistory(RequestMessage requestMessage);
        Task<ResponseMessage> SavePatientHistory(RequestMessage requestMessage);
        Task<ResponseMessage> GetPatientHistoryById(RequestMessage requestMessage);

    }
}
