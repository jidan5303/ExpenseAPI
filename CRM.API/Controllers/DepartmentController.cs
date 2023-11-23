using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService userService)
        {
            this._departmentService = userService;
        }

        [HttpPost("GetAllDepartment")]
        public async Task<ResponseMessage> GetAllDepartment(RequestMessage requestMessage)
        {
            return await _departmentService.GetAllDepartment(requestMessage);
        }

        [HttpPost("GeDepartmentById")]
        public async Task<ResponseMessage> GetDepartmentById(RequestMessage requestMessage)
        {
            return await this._departmentService.GetDepartmentById(requestMessage);
        }

        [HttpPost("SaveDepartment")]
        public async Task<ResponseMessage> SaveDepartment(RequestMessage requestMessage)
        {
            return await _departmentService.SaveDepartment(requestMessage);
        }
    }
}
