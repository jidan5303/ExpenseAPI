using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/DefaultTable")]
    [ApiController]
    public class DefaultTableController : ControllerBase
    {
        private readonly IDefaultTableService _defaultTableService;
        public DefaultTableController(IDefaultTableService userService)
        {
            this._defaultTableService = userService;
        }

        [HttpPost("GetAllDefaultTable")]
        public async Task<ResponseMessage> GetAllDefaultTable(RequestMessage requestMessage)
        {
            return await _defaultTableService.GetAllDefaultTable(requestMessage);
        }

        [HttpPost("GeDefaultTableById")]
        public async Task<ResponseMessage> GetDefaultTableById(RequestMessage requestMessage)
        {
            return await this._defaultTableService.GetDefaultTableById(requestMessage);
        }

        [HttpPost("SaveDefaultTable")]
        public async Task<ResponseMessage> SaveDefaultTable(RequestMessage requestMessage)
        {
            return await _defaultTableService.SaveDefaultTable(requestMessage);
        }
    }
}
