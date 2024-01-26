using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
    public interface IDepositService
    {
        Task<ResponseMessage> GetAllDeposit(RequestMessage requestMessage);
        Task<ResponseMessage> SaveDeposit(RequestMessage requestMessage);
        Task<ResponseMessage> DeleteDeposit(RequestMessage requestMessage);
        Task<ResponseMessage> GetBalance(RequestMessage requestMessage);
        Task<ResponseMessage> GetMonthlyExpenseSum(RequestMessage requestMessage);
    }
}
