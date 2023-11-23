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
    public interface IActionsService
    {
        Task<ResponseMessage> GetAllActions(RequestMessage requestMessage);
        Task<ResponseMessage> SaveActions(RequestMessage requestMessage);
        Task<ResponseMessage> GetActionsById(RequestMessage requestMessage);

    }
}
