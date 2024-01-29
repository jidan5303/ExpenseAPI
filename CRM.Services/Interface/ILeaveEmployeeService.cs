using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
    public interface ILeaveEmployeeService
    {
        Task<ResponseMessage> SaveEmployee(RequestMessage requestMessage);
        Task<ResponseMessage> GetEmployee(RequestMessage requestMessage);
        Task<ResponseMessage> DeleteEmployee(RequestMessage requestMessage);
        Task<ResponseMessage> GetLeaveBalance(RequestMessage requestMessage);
        Task<ResponseMessage> GetLeaveBalanceByYear(RequestMessage requestMessage);
        Task<ResponseMessage> GetEmployeeByUserName(RequestMessage requestMessage);
    }
}
