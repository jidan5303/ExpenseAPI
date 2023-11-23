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
    public interface IDefaultTableColumnService
    {
        Task<ResponseMessage> GetAllDefaultTableColumn(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDefaultTableColumn(RequestMessage requestMessage);
        Task<ResponseMessage> GetDefaultTableColumnById(RequestMessage requestMessage);

    }
}
