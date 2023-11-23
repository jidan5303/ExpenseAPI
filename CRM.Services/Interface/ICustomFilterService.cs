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
    public interface ICustomFilterService
    {
        Task<ResponseMessage> GetAllCustomFilter(RequestMessage requestMessage);
        Task<ResponseMessage> SaveCustomFilter(RequestMessage requestMessage);
        Task<ResponseMessage> GetCustomFilterById(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllCustomFilterByUserNameAndPageName(RequestMessage requestMessage);
        Task<ResponseMessage> DeleteCustomFilter(RequestMessage requestMessage);
        Task<ResponseMessage> SetDefaultCustomFilter(RequestMessage requestMessage);
        Task<ResponseMessage> GetDefaultCustomFilter(RequestMessage requestMessage);
        Task<ResponseMessage> SaveShareCustomFilter(RequestMessage requestMessage);
        Task<ResponseMessage> SaveShareAllDepCustomFilter(RequestMessage requestMessage);
    }
}
