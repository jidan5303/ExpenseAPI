using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/DepartmentTask")]
    [ApiController]
    public class DepartmentTaskController : ControllerBase
    {
        private readonly IDepartmentTaskService _departmentTaskService;
        public DepartmentTaskController(IDepartmentTaskService userService)
        {
            this._departmentTaskService = userService;
        }

        [HttpPost("GetAllDepartmentTask")]
        public async Task<ResponseMessage> GetAllDepartmentTask(RequestMessage requestMessage)
        {
            return await _departmentTaskService.GetAllDepartmentTask(requestMessage);
        }

        [HttpPost("GetDepartmentTaskById")]
        public async Task<ResponseMessage> GetSystemUserById(RequestMessage requestMessage)
        {
            return await this._departmentTaskService.GetDepartmentTaskById(requestMessage);
        }

        [HttpPost("SaveDepartmentTask")]
        public async Task<ResponseMessage> SaveDepartmentTask(RequestMessage requestMessage)
        {
            return await _departmentTaskService.SaveDepartmentTask(requestMessage);
        }
    }
}
