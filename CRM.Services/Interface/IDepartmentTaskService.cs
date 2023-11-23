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
    public interface IDepartmentTaskService
    {
        Task<ResponseMessage> GetAllDepartmentTask(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDepartmentTask(RequestMessage requestMessage);
        Task<ResponseMessage> GetDepartmentTaskById(RequestMessage requestMessage);

    }
}
