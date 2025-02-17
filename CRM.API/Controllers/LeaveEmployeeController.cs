﻿using CRM.Common.DTO;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/LeaveEmployee")]
    [ApiController]
    public class LeaveEmployeeController : ControllerBase
    {
        private readonly ILeaveEmployeeService _employeeService;

        public LeaveEmployeeController(ILeaveEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("GetAllEmployee")]
        public async Task<ResponseMessage> GetAllEmployee(RequestMessage requestMessage)
        {
            return await _employeeService.GetEmployee(requestMessage);
        }

        [HttpPost("GetEmployeeByUserName")]
        public async Task<ResponseMessage> GetEmployeeByUserName(RequestMessage requestMessage)
        {
            return await _employeeService.GetEmployeeByUserName(requestMessage);
        }

        [HttpPost("SaveEmployee")]
        public async Task<ResponseMessage> SaveEmployee(RequestMessage requestMessage)
        {
            return await _employeeService.SaveEmployee(requestMessage);
        }

        [HttpPost("DeleteEmployee")]
        public async Task<ResponseMessage> DeleteEmployee(RequestMessage requestMessage)
        {
            return await _employeeService.DeleteEmployee(requestMessage);
        }

        [HttpPost("GetLeaveBalance")]
        public async Task<ResponseMessage> GetLeaveBalance(RequestMessage requestMessage)
        {
            return await _employeeService.GetLeaveBalance(requestMessage);
        }

        [HttpPost("GetLeaveBalanceByYear")]
        public async Task<ResponseMessage> GetLeaveBalanceByYear(RequestMessage requestMessage)
        {
            return await _employeeService.GetLeaveBalanceByYear(requestMessage);
        }
    }
}
