using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/DepartmentTaskHistory")]
    [ApiController]
    public class DepartmentTaskHistoryController : ControllerBase
    {
        private readonly IDepartmentTaskHistoryService _departmentTaskHistoryService;
        public DepartmentTaskHistoryController(IDepartmentTaskHistoryService userService)
        {
            this._departmentTaskHistoryService = userService;
        }

        [HttpPost("GetAllDepartmentTaskHistory")]
        public async Task<ResponseMessage> GetAllDepartmentTaskHistory(RequestMessage requestMessage)
        {
            return await _departmentTaskHistoryService.GetAllDepartmentTaskHistory(requestMessage);
        }

        [HttpPost("GetDepartmentTaskHistoryById")]
        public async Task<ResponseMessage> GetSystemUserById(RequestMessage requestMessage)
        {
            return await this._departmentTaskHistoryService.GetDepartmentTaskHistoryById(requestMessage);
        }

        [HttpPost("SaveDepartmentTaskHistory")]
        public async Task<ResponseMessage> SaveDepartmentTaskHistory(RequestMessage requestMessage)
        {
            return await _departmentTaskHistoryService.SaveDepartmentTaskHistory(requestMessage);
        }
    }
}
