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
    public interface ICaregiverAttachmentService
    {
        Task<ResponseMessage> GetCaregiverAttachmentByCaregiverID(RequestMessage requestMessage);
        Task<ResponseMessage> SaveCaregiverAttachment(RequestMessage requestMessage);
        Task<ResponseMessage> RemoveCaregiverAttachment(RequestMessage requestMessage);

    }
}
