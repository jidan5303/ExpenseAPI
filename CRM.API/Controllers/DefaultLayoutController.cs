using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/DefaultLayout")]
    [ApiController]
    public class DefaultLayoutController : ControllerBase
    {
        private readonly IDefaultLayoutService _defaultLayoutService;
        public DefaultLayoutController(IDefaultLayoutService userService)
        {
            this._defaultLayoutService = userService;
        }

        [HttpPost("GetAllDefaultLayout")]
        public async Task<ResponseMessage> GetAllDefaultLayout(RequestMessage requestMessage)
        {
            return await _defaultLayoutService.GetAllDefaultLayout(requestMessage);
        }

        [HttpPost("GeDefaultLayoutById")]
        public async Task<ResponseMessage> GetDefaultLayoutById(RequestMessage requestMessage)
        {
            return await this._defaultLayoutService.GetDefaultLayoutById(requestMessage);
        }

        [HttpPost("SaveDefaultLayout")]
        public async Task<ResponseMessage> SaveDefaultLayout(RequestMessage requestMessage)
        {
            return await _defaultLayoutService.SaveDefaultLayout(requestMessage);
        }

        [HttpPost("SaveDefaultLayoutAndDetails")]
        public async Task<ResponseMessage> SaveDefaultLayoutAndDetails(RequestMessage requestMessage)
        {
            return await _defaultLayoutService.SaveDefaultLayoutAndDetails(requestMessage);
        }



        [HttpPost("GetAllDefaultLayoutInitData")]
        public async Task<ResponseMessage> GetAllDefaultLayoutInitData(RequestMessage requestMessage)
        {
            return await _defaultLayoutService.GetAllDefaultLayoutInitData(requestMessage);
        }

        [HttpPost("GeDefaultTableColumnByDefaultTableID")]
        public async Task<ResponseMessage> GeDefaultTableColumnByDefaultTableID(RequestMessage requestMessage)
        {
            return await this._defaultLayoutService.GeDefaultTableColumnByDefaultTableID(requestMessage);
        }
    }
}
