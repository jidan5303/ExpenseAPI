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
    public interface IDefaultTableService
    {
        Task<ResponseMessage> GetAllDefaultTable(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDefaultTable(RequestMessage requestMessage);
        Task<ResponseMessage> GetDefaultTableById(RequestMessage requestMessage);

    }
}
