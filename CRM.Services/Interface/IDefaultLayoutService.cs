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
    public interface IDefaultLayoutService
    {
        Task<ResponseMessage> GetAllDefaultLayout(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDefaultLayout(RequestMessage requestMessage);
        Task<ResponseMessage> GetDefaultLayoutById(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllDefaultLayoutInitData(RequestMessage requestMessage);
        Task<ResponseMessage> GeDefaultTableColumnByDefaultTableID(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDefaultLayoutAndDetails(RequestMessage requestMessage);


    }
}
