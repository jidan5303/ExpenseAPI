using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Actions")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly IActionsService _actionsService;
        public ActionsController(IActionsService userService)
        {
            this._actionsService = userService;
        }

        [HttpPost("GetAllActions")]
        public async Task<ResponseMessage> GetAllActions(RequestMessage requestMessage)
        {
            return await _actionsService.GetAllActions(requestMessage);
        }

        [HttpPost("GeActionsById")]
        public async Task<ResponseMessage> GetActionsById(RequestMessage requestMessage)
        {
            return await this._actionsService.GetActionsById(requestMessage);
        }

        [HttpPost("SaveActions")]
        public async Task<ResponseMessage> SaveActions(RequestMessage requestMessage)
        {
            return await _actionsService.SaveActions(requestMessage);
        }
    }
}
