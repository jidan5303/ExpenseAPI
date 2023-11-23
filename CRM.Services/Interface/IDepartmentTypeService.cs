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
    public interface IDepartmentTypeService
    {
        Task<ResponseMessage> GetAllDepartmentType(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDepartmentType(RequestMessage requestMessage);
        Task<ResponseMessage> GetDepartmentTypeById(RequestMessage requestMessage);

    }
}
