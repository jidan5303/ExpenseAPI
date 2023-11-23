using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/DepartmentType")]
    [ApiController]
    public class DepartmentTypeController : ControllerBase
    {
        private readonly IDepartmentTypeService _departmentTypeService;
        public DepartmentTypeController(IDepartmentTypeService userService)
        {
            this._departmentTypeService = userService;
        }

        [HttpPost("GetAllDepartmentType")]
        public async Task<ResponseMessage> GetAllDepartmentType(RequestMessage requestMessage)
        {
            return await _departmentTypeService.GetAllDepartmentType(requestMessage);
        }

        [HttpPost("GeDepartmentTypeById")]
        public async Task<ResponseMessage> GetDepartmentTypeById(RequestMessage requestMessage)
        {
            return await this._departmentTypeService.GetDepartmentTypeById(requestMessage);
        }

        [HttpPost("SaveDepartmentType")]
        public async Task<ResponseMessage> SaveDepartmentType(RequestMessage requestMessage)
        {
            return await _departmentTypeService.SaveDepartmentType(requestMessage);
        }
    }
}
