using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/KnowledgeBaseHistory")]
    [ApiController]
    public class KnowledgeBaseHistoryController : ControllerBase
    {
        private readonly IKnowledgeBaseHistoryService _knowledgeBaseHistoryService;
        public KnowledgeBaseHistoryController(IKnowledgeBaseHistoryService userService)
        {
            this._knowledgeBaseHistoryService = userService;
        }

        [HttpPost("GetAllKnowledgeBaseHistory")]
        public async Task<ResponseMessage> GetAllKnowledgeBaseHistory(RequestMessage requestMessage)
        {
            return await _knowledgeBaseHistoryService.GetAllKnowledgeBaseHistory(requestMessage);
        }

        [HttpPost("GeKnowledgeBaseHistoryById")]
        public async Task<ResponseMessage> GetKnowledgeBaseHistoryById(RequestMessage requestMessage)
        {
            return await this._knowledgeBaseHistoryService.GetKnowledgeBaseHistoryById(requestMessage);
        }

        [HttpPost("SaveKnowledgeBaseHistory")]
        public async Task<ResponseMessage> SaveKnowledgeBaseHistory(RequestMessage requestMessage)
        {
            return await _knowledgeBaseHistoryService.SaveKnowledgeBaseHistory(requestMessage);
        }
    }
}
