using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/UserSession")]
    [ApiController]
    public class UserSessionController : ControllerBase
    {
        private readonly IUserSessionService _userSessionService;
        public UserSessionController(IUserSessionService userService)
        {
            this._userSessionService = userService;
        }

        [HttpPost("GetAllUserSession")]
        public async Task<ResponseMessage> GetAllUserSession(RequestMessage requestMessage)
        {
            return await _userSessionService.GetAllUserSession(requestMessage);
        }

        [HttpPost("GeUserSessionById")]
        public async Task<ResponseMessage> GetUserSessionById(RequestMessage requestMessage)
        {
            return await this._userSessionService.GetUserSessionById(requestMessage);
        }

        [HttpPost("SaveUserSession")]
        public async Task<ResponseMessage> SaveUserSession(RequestMessage requestMessage)
        {
            return await _userSessionService.SaveUserSession(requestMessage);
        }
    }
}
