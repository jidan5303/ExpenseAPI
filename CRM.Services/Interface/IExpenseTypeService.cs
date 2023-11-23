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
    public interface IExpenseTypeService
    {
        Task<ResponseMessage> GetAllExpenseType(RequestMessage requestMessage);
        Task<ResponseMessage> GetExpenseTypeByExpenseCategoryId(RequestMessage requestMessage);
        Task<ResponseMessage> SaveExpenseType(RequestMessage requestMessage);
        Task<ResponseMessage> GetExpenseTypeById(RequestMessage requestMessage);

    }
}
