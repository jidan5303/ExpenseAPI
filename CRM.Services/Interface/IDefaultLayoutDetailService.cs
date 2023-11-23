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
    public interface IDefaultLayoutDetailService
    {
        Task<ResponseMessage> GetAllDefaultLayoutDetail(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDefaultLayoutDetail(RequestMessage requestMessage);
        Task<ResponseMessage> GetDefaultLayoutDetailById(RequestMessage requestMessage);

    }
}
