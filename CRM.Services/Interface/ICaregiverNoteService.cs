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
    public interface ICaregiverNoteService
    {
        Task<ResponseMessage> SaveCaregiverNote(RequestMessage requestMessage);
        Task<ResponseMessage> GetCaregiverNoteByCaregiverID(RequestMessage requestMessage);

    }
}
