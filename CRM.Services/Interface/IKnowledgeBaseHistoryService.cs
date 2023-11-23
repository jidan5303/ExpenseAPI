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
    public interface IKnowledgeBaseHistoryService
    {
        Task<ResponseMessage> GetAllKnowledgeBaseHistory(RequestMessage requestMessage);
        Task<ResponseMessage> SaveKnowledgeBaseHistory(RequestMessage requestMessage);
        Task<ResponseMessage> GetKnowledgeBaseHistoryById(RequestMessage requestMessage);

    }
}
