using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/KnowledgeBase")]
    [ApiController]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IKnowledgeBaseService _knowledgeBaseService;
        public KnowledgeBaseController(IKnowledgeBaseService userService)
        {
            this._knowledgeBaseService = userService;
        }

        [HttpPost("GetAllKnowledgeBase")]
        public async Task<ResponseMessage> GetAllKnowledgeBase(RequestMessage requestMessage)
        {
            return await _knowledgeBaseService.GetAllKnowledgeBase(requestMessage);
        }

        [HttpPost("GeKnowledgeBaseById")]
        public async Task<ResponseMessage> GetKnowledgeBaseById(RequestMessage requestMessage)
        {
            return await this._knowledgeBaseService.GetKnowledgeBaseById(requestMessage);
        }

        [HttpPost("SaveKnowledgeBase")]
        public async Task<ResponseMessage> SaveKnowledgeBase(RequestMessage requestMessage)
        {
            return await _knowledgeBaseService.SaveKnowledgeBase(requestMessage);
        }

        [HttpPost("GeKnowledgeBaseByDepartmentID")]
        public async Task<ResponseMessage> GeKnowledgeBaseByDepartmentID(RequestMessage requestMessage)
        {
            return await this._knowledgeBaseService.GeKnowledgeBaseByDepartmentID(requestMessage);
        }

        [HttpPost("RemoveKnowledgeBase")]
        public async Task<ResponseMessage> RemoveKnowledgeBase(RequestMessage requestMessage)
        {
            return await this._knowledgeBaseService.RemoveKnowledgeBase(requestMessage);
        }

    }
}
