using CRM.Common.DTO;
using CRM.Common.Models;
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
    public interface IExpenseAttachmentService
    {
        Task<ResponseMessage> SaveExpenseAttachment(ExpenseAttachment objExpenseAttachment, int loggedInUserID);
    }
}
