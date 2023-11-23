using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/DefaultTableColumn")]
    [ApiController]
    public class DefaultTableColumnController : ControllerBase
    {
        private readonly IDefaultTableColumnService _defaultTableColumnService;
        public DefaultTableColumnController(IDefaultTableColumnService userService)
        {
            this._defaultTableColumnService = userService;
        }

        [HttpPost("GetAllDefaultTableColumn")]
        public async Task<ResponseMessage> GetAllDefaultTableColumn(RequestMessage requestMessage)
        {
            return await _defaultTableColumnService.GetAllDefaultTableColumn(requestMessage);
        }

        [HttpPost("GeDefaultTableColumnById")]
        public async Task<ResponseMessage> GetDefaultTableColumnById(RequestMessage requestMessage)
        {
            return await this._defaultTableColumnService.GetDefaultTableColumnById(requestMessage);
        }


        [HttpPost("SaveDefaultTableColumn")]
        public async Task<ResponseMessage> SaveDefaultTableColumn(RequestMessage requestMessage)
        {
            return await _defaultTableColumnService.SaveDefaultTableColumn(requestMessage);
        }
    }
}
