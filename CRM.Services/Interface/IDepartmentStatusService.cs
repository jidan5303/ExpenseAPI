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
    public interface IDepartmentStatusService
    {
        Task<ResponseMessage> GetAllDepartmentStatus(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDepartmentStatus(RequestMessage requestMessage);
        Task<ResponseMessage> GetDepartmentStatusById(RequestMessage requestMessage);

    }
}
