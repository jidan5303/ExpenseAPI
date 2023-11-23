using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/CustomFilter")]
    [ApiController]
    public class CustomFilterController : ControllerBase
    {
        private readonly ICustomFilterService _CustomFilterService;
        public CustomFilterController(ICustomFilterService userService)
        {
            this._CustomFilterService = userService;
        }

        [HttpPost("GetAllCustomFilter")]
        public async Task<ResponseMessage> GetAllCustomFilter(RequestMessage requestMessage)
        {
            return await _CustomFilterService.GetAllCustomFilter(requestMessage);
        }

        [HttpPost("GetCustomFilterById")]
        public async Task<ResponseMessage> GetCustomFilterById(RequestMessage requestMessage)
        {
            return await this._CustomFilterService.GetCustomFilterById(requestMessage);
        }

        [HttpPost("SaveCustomFilter")]
        public async Task<ResponseMessage> SaveCustomFilter(RequestMessage requestMessage)
        {
            return await _CustomFilterService.SaveCustomFilter(requestMessage);
        }

        [HttpPost("GetAllCustomFilterByUserNameAndPageName")]
        public async Task<ResponseMessage> GetAllCustomFilterByUserNameAndPageName(RequestMessage requestMessage)
        {
            return await this._CustomFilterService.GetAllCustomFilterByUserNameAndPageName(requestMessage);
        }

        [HttpPost("DeleteCustomFilter")]
        public async Task<ResponseMessage> DeleteCustomFilter(RequestMessage requestMessage)
        {
            return await this._CustomFilterService.DeleteCustomFilter(requestMessage);
        }

        [HttpPost("SetDefaultCustomFilter")]
        public async Task<ResponseMessage> SetDefaultCustomFilter(RequestMessage requestMessage)
        {
            return await this._CustomFilterService.SetDefaultCustomFilter(requestMessage);
        }

        [HttpPost("GetDefaultCustomFilter")]
        public async Task<ResponseMessage> GetDefaultCustomFilter(RequestMessage requestMessage)
        {
            return await this._CustomFilterService.GetDefaultCustomFilter(requestMessage);
        }

        [HttpPost("ShareCustomFilter")]
        public async Task<ResponseMessage> SaveShareCustomFilter(RequestMessage requestMessage)
        {
            return await _CustomFilterService.SaveShareCustomFilter(requestMessage);
        }

        [HttpPost("ShareAllDepCustomFilter")]
        public async Task<ResponseMessage> SaveShareAllDepCustomFilter(RequestMessage requestMessage)
        {
            return await _CustomFilterService.SaveShareAllDepCustomFilter(requestMessage);
        }

    }
}
