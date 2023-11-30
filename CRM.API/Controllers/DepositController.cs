using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Deposit")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly IDepositService _depositService;
        public DepositController(IDepositService depositService)
        {
            _depositService = depositService;
        }

        [HttpPost("GetAllDeposit")]
        public async Task<ResponseMessage> GetAllDeposit(RequestMessage requestMessage)
        {
            return await _depositService.GetAllDeposit(requestMessage);
        }

        [HttpPost("SaveDeposit")]
        public async Task<ResponseMessage> SaveDeposit(RequestMessage requestMessage)
        {
            return await _depositService.SaveDeposit(requestMessage);
        }

        [HttpPost("DeleteDeposit")]
        public async Task<ResponseMessage> DeleteDeposit(RequestMessage requestMessage)
        {
            return await _depositService.DeleteDeposit(requestMessage);
        }

        [HttpPost("GetBalance")]
        public async Task<ResponseMessage> GetBalance(RequestMessage requestMessage)
        {
            return await _depositService.GetBalance(requestMessage);
        }
    }
}
