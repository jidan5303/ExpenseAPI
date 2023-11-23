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
    public interface ISystemUserService
    {
        Task<ResponseMessage> GetAllSystemUser(RequestMessage requestMessage);
        Task<ResponseMessage> SaveSystemUser(RequestMessage requestMessage);
        Task<ResponseMessage> GetSystemUserById(RequestMessage requestMessage);
        Task<ResponseMessage> GetSystemUsersByRole(RequestMessage requestMessage);
        Task<ResponseMessage> GetSystemUserByUserName(RequestMessage requestMessage);
        Task<ResponseMessage> GetSystemUserByEmail(RequestMessage requestMessage);
        Task<ResponseMessage> DeleteSystemUser(RequestMessage requestMessage);
        Task<ResponseMessage> GetSystemUserByDepartmentId(RequestMessage requestMessage);

    }
}
