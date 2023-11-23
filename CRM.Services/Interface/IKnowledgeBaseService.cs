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
    public interface IKnowledgeBaseService
    {
        Task<ResponseMessage> GetAllKnowledgeBase(RequestMessage requestMessage);
        Task<ResponseMessage> SaveKnowledgeBase(RequestMessage requestMessage);
        Task<ResponseMessage> GetKnowledgeBaseById(RequestMessage requestMessage);
        Task<ResponseMessage> GeKnowledgeBaseByDepartmentID(RequestMessage requestMessage);
        Task<ResponseMessage> RemoveKnowledgeBase(RequestMessage requestMessage);

    }
}
