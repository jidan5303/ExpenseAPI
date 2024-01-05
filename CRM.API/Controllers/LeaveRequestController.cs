using CRM.Common.DTO;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/LeaveRequest")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpPost("GetAllLeaveRequest")]
        public async Task<ResponseMessage> GetAllLeaveRequest(RequestMessage requestMessage)
        {
            return await _leaveRequestService.GetAllLeaveRequest(requestMessage);
        }

        [HttpPost("SaveLeaveRequest")]
        public async Task<ResponseMessage> SaveLeaveRequest(RequestMessage requestMessage)
        {
            return await _leaveRequestService.SaveLeaveRequest(requestMessage);
        }

        [HttpPost("DeleteLeaveRequest")]
        public async Task<ResponseMessage> DeleteLeaveRequest(RequestMessage requestMessage)
        {
            return await _leaveRequestService.DeleteLeaveRequest(requestMessage);
        }

        [HttpPost("AcceptLeaveRequest")]
        public async Task<ResponseMessage> AcceptLeaveRequest(RequestMessage requestMessage)
        {
            return await _leaveRequestService.AcceptLeaveRequest(requestMessage);
        }

        [HttpPost("RejectLeaveRequest")]
        public async Task<ResponseMessage> RejectLeaveRequest(RequestMessage requestMessage)
        {
            return await _leaveRequestService.RejectLeaveRequest(requestMessage);
        }

        [HttpPost("GetAllLeaveType")]
        public async Task<ResponseMessage> GetAllLeaveType(RequestMessage requestMessage)
        {
            return await _leaveRequestService.GetAllLeaveType(requestMessage);
        }
    }
}
