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
    public interface IExpenseService
    {
        Task<ResponseMessage> GetAllDailyExpenseByMonth(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllMonthlyExpenseByMonth(RequestMessage requestMessage);
        Task<ResponseMessage> GetMonthlySummaryReport(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllExpense(RequestMessage requestMessage);
        //Task<ResponseMessage> SaveExpense(RequestMessage requestMessage);
        Task<ResponseMessage> SaveExpense(RequestMessage requestMessage, string rootUrl);
        Task<ResponseMessage> UpdateExpense(RequestMessage requestMessage);
        Task<ResponseMessage> GetExpenseById(RequestMessage requestMessage);
        Task<ResponseMessage> GetExpenseByDate(RequestMessage requestMessage);
        Task<ResponseMessage> DeleteExpense(RequestMessage requestMessage);
        Task<ResponseMessage> GetAttachment(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllDailyExpense(RequestMessage requestMessage);

    }
}
