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
    public interface ITagService
    {
        Task<ResponseMessage> GetAllTag(RequestMessage requestMessage);
        Task<ResponseMessage> SaveTag(RequestMessage requestMessage);
        Task<ResponseMessage> GetTagById(RequestMessage requestMessage);
        Task<ResponseMessage> GetTagByDepartment(RequestMessage requestMessage);
        Task<ResponseMessage> DeleteTag(RequestMessage requestMessage);

    }
}
