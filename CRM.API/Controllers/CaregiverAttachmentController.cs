using CRM.Common.DTO;
using CRM.Services;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/CaregiverAttachment")]
    [ApiController]
    public class CaregiverAttachmentController : ControllerBase
    {
        private readonly ICaregiverAttachmentService _caregiverAttachmentService;
        public CaregiverAttachmentController(ICaregiverAttachmentService caregiverAttachmentService)
        {
            _caregiverAttachmentService = caregiverAttachmentService;
        }

        [HttpPost("GetCaregiverAttachmentByCaregiverID")]
        public async Task<ResponseMessage> GetCaregiverAttachmentByCaregiverID(RequestMessage requestMessage)
        {
            return await _caregiverAttachmentService.GetCaregiverAttachmentByCaregiverID(requestMessage);
        }

        [HttpPost("SaveCaregiverAttachment")]
        public async Task<ResponseMessage> SaveCaregiverAttachment(RequestMessage requestMessage)
        {
            return await _caregiverAttachmentService.SaveCaregiverAttachment(requestMessage);
        }

        [HttpPost("RemoveCaregiverAttachment")]
        public async Task<ResponseMessage> RemoveCaregiverAttachment(RequestMessage requestMessage)
        {
            return await _caregiverAttachmentService.RemoveCaregiverAttachment(requestMessage);
        }



    }
}
