using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService userService)
        {
            this._roleService = userService;
        }

        [HttpPost("GetAllRole")]
        public async Task<ResponseMessage> GetAllRole(RequestMessage requestMessage)
        {
            return await _roleService.GetAllRole(requestMessage);
        }

        [HttpPost("GeRoleById")]
        public async Task<ResponseMessage> GetRoleById(RequestMessage requestMessage)
        {
            return await this._roleService.GetRoleById(requestMessage);
        }

        [HttpPost("SaveRole")]
        public async Task<ResponseMessage> SaveRole(RequestMessage requestMessage)
        {
            return await _roleService.SaveRole(requestMessage);
        }  
        
        [HttpPost("DeleteRole")]
        public async Task<ResponseMessage> DeleteRole(RequestMessage requestMessage)
        {
            return await _roleService.DeleteRole(requestMessage);
        }
    }
}
