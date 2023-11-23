using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/SystemUser")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        private readonly ISystemUserService _userService;

        public SystemUserController(ISystemUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("GetAllSystemUser")]
        public async Task<ResponseMessage> GetAllSystemUsers(RequestMessage requestMessage)
        {
            return await this._userService.GetAllSystemUser(requestMessage);
        }

        [HttpPost("GetSystemUserById")]
        public async Task<ResponseMessage> GetSystemUserById(RequestMessage requestMessage)
        {
            return await this._userService.GetSystemUserById(requestMessage);
        }

        [HttpPost("GetSystemUsersByRole")]
        public async Task<ResponseMessage> GetSystemUserByRole(RequestMessage requestMessage)
        {
            return await this._userService.GetSystemUsersByRole(requestMessage);
        }

        [HttpPost("SaveSystemUser")]
        public async Task<ResponseMessage> SaveSystemUsers(RequestMessage requestMessage)
        {
            return await this._userService.SaveSystemUser(requestMessage);
        }

        [HttpPost("GetSystemUserByUserName")]
        public async Task<ResponseMessage> GetSystemUserByUserName(RequestMessage requestMessage)
        {
            return await this._userService.GetSystemUserByUserName(requestMessage);
        }

        [HttpPost("GetSystemUserByEmail")]
        public async Task<ResponseMessage> GetSystemUserByEmail(RequestMessage requestMessage)
        {
            return await this._userService.GetSystemUserByEmail(requestMessage);
        } 
        
        [HttpPost("DeleteSystemUser")]
        public async Task<ResponseMessage> DeleteSystemUser(RequestMessage requestMessage)
        {
            return await this._userService.DeleteSystemUser(requestMessage);
        }

        [HttpPost("GetSystemUserByDepartmentId")]
        public async Task<ResponseMessage> GetSystemUserByDepartmentId(RequestMessage requestMessage)
        {
            return await this._userService.GetSystemUserByDepartmentId(requestMessage);
        }

    }
}
