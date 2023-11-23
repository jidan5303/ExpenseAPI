using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Permission")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService userService)
        {
            this._permissionService = userService;
        }

        [HttpPost("GetAllPermission")]
        public async Task<ResponseMessage> GetAllPermission(RequestMessage requestMessage)
        {
            return await _permissionService.GetAllPermission(requestMessage);
        }

        [HttpPost("GePermissionById")]
        public async Task<ResponseMessage> GetPermissionById(RequestMessage requestMessage)
        {
            return await this._permissionService.GetPermissionById(requestMessage);
        }

        [HttpPost("SavePermission")]
        public async Task<ResponseMessage> SavePermission(RequestMessage requestMessage)
        {
            return await _permissionService.SavePermission(requestMessage);
        }

        [HttpPost("SequencePermissions")]
        public async Task<ResponseMessage> SequencePermissions(RequestMessage requestMessage)
        {
            return await _permissionService.SequencePermissions(requestMessage);
        }
    }
}
