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
    public interface IExpenseCategoryService
    {
        Task<ResponseMessage> GetAllExpenseCategory(RequestMessage requestMessage);
        Task<ResponseMessage> SaveExpenseCategory(RequestMessage requestMessage);
        Task<ResponseMessage> GetExpenseCategoryById(RequestMessage requestMessage);

    }
}
