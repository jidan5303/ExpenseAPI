using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/DepartmentStatus")]
    [ApiController]
    public class DepartmentStatusController : ControllerBase
    {
        private readonly IDepartmentStatusService _departmentStatusService;
        public DepartmentStatusController(IDepartmentStatusService userService)
        {
            this._departmentStatusService = userService;
        }

        [HttpPost("GetAllDepartmentStatus")]
        public async Task<ResponseMessage> GetAllDepartmentStatus(RequestMessage requestMessage)
        {
            return await _departmentStatusService.GetAllDepartmentStatus(requestMessage);
        }
        [HttpPost("GeDepartmentStatusById")]
        public async Task<ResponseMessage> GetSystemUserById(RequestMessage requestMessage)
        {
            return await this._departmentStatusService.GetDepartmentStatusById(requestMessage);
        }
        [HttpPost("SaveDepartmentStatus")]
        public async Task<ResponseMessage> SaveDepartmentStatus(RequestMessage requestMessage)
        {
            return await _departmentStatusService.SaveDepartmentStatus(requestMessage);
        }
    }
}
