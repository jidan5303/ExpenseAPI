using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/DefaultLayoutDetail")]
    [ApiController]
    public class DefaultLayoutDetailController : ControllerBase
    {
        private readonly IDefaultLayoutDetailService _defaultLayoutDetailService;
        public DefaultLayoutDetailController(IDefaultLayoutDetailService userService)
        {
            this._defaultLayoutDetailService = userService;
        }

        [HttpPost("GetAllDefaultLayoutDetail")]
        public async Task<ResponseMessage> GetAllDefaultLayoutDetail(RequestMessage requestMessage)
        {
            return await _defaultLayoutDetailService.GetAllDefaultLayoutDetail(requestMessage);
        }

        [HttpPost("GeDefaultLayoutDetailById")]
        public async Task<ResponseMessage> GetDefaultLayoutDetailById(RequestMessage requestMessage)
        {
            return await this._defaultLayoutDetailService.GetDefaultLayoutDetailById(requestMessage);
        }

        [HttpPost("SaveDefaultLayoutDetail")]
        public async Task<ResponseMessage> SaveDefaultLayoutDetail(RequestMessage requestMessage)
        {
            return await _defaultLayoutDetailService.SaveDefaultLayoutDetail(requestMessage);
        }
    }
}
