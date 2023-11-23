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
    public interface IEmployeeService
    {
        Task<ResponseMessage> GetAllEmployee(RequestMessage requestMessage);
        Task<ResponseMessage> SaveEmployee(RequestMessage requestMessage);
        Task<ResponseMessage> GetEmployeeById(RequestMessage requestMessage);

    }
}
