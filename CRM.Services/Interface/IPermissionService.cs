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
    public interface IPermissionService
    {
        Task<ResponseMessage> GetAllPermission(RequestMessage requestMessage);
        Task<ResponseMessage> SavePermission(RequestMessage requestMessage);
        Task<ResponseMessage> SequencePermissions(RequestMessage requestMessage);
        Task<ResponseMessage> GetPermissionById(RequestMessage requestMessage);
        Task<ResponseMessage> GetPermissionByRoleId(RequestMessage requestMessage);

    }
}
