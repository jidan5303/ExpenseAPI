using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Tag")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService userService)
        {
            this._tagService = userService;
        }

        [HttpPost("GetAllTag")]
        public async Task<ResponseMessage> GetAllTag(RequestMessage requestMessage)
        {
            return await _tagService.GetAllTag(requestMessage);
        }

        [HttpPost("GeTagById")]
        public async Task<ResponseMessage> GetTagById(RequestMessage requestMessage)
        {
            return await this._tagService.GetTagById(requestMessage);
        }
        
        [HttpPost("GetTagByDepartment")]
        public async Task<ResponseMessage> GetTagByDepartment(RequestMessage requestMessage)
        {
            return await this._tagService.GetTagByDepartment(requestMessage);
        }

        [HttpPost("SaveTag")]
        public async Task<ResponseMessage> SaveTag(RequestMessage requestMessage)
        {
            return await _tagService.SaveTag(requestMessage);
        }
        [HttpPost("DeleteTag")]
        public async Task<ResponseMessage> DeleteTag(RequestMessage requestMessage)
        {
            return await _tagService.DeleteTag(requestMessage);
        }
    }
}
