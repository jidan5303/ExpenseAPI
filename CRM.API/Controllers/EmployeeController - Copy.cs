using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService userService)
        {
            this._employeeService = userService;
        }

        [HttpPost("GetAllEmployee")]
        public async Task<ResponseMessage> GetAllEmployee(RequestMessage requestMessage)
        {
            return await _employeeService.GetAllEmployee(requestMessage);
        }

        [HttpPost("GeEmployeeById")]
        public async Task<ResponseMessage> GetEmployeeById(RequestMessage requestMessage)
        {
            return await this._employeeService.GetEmployeeById(requestMessage);
        }

        [HttpPost("SaveEmployee")]
        public async Task<ResponseMessage> SaveEmployee(RequestMessage requestMessage)
        {
            return await _employeeService.SaveEmployee(requestMessage);
        }
    }
}
