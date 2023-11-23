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
    public interface IDepartmentTaskHistoryService
    {
        Task<ResponseMessage> GetAllDepartmentTaskHistory(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDepartmentTaskHistory(RequestMessage requestMessage);
        Task<ResponseMessage> GetDepartmentTaskHistoryById(RequestMessage requestMessage);

    }
}
