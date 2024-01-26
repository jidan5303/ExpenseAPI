using CRM.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interface
{
    public interface ILeaveRequestService
    {
        Task<ResponseMessage> SaveLeaveRequest(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllLeaveRequest(RequestMessage requestMessage);
        Task<ResponseMessage> DeleteLeaveRequest(RequestMessage requestMessage);
        Task<ResponseMessage> AcceptLeaveRequest(RequestMessage requestMessage);
        Task<ResponseMessage> RejectLeaveRequest(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllLeaveType(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllLeaveRequestByYear(RequestMessage requestMessage);
        Task<ResponseMessage> GetAllLeaveRequestByID(RequestMessage requestMessage);
    }
}
