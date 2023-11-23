using CRM.Common.DTO;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/Organization")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            this._organizationService = organizationService;
        }

        [HttpPost("GetAllOrganization")]
        public async Task<ResponseMessage> GetAllOrganization(RequestMessage requestMessage)
        {
            return await _organizationService.GetAllOrganization(requestMessage);
        }

        [HttpPost("GeOrganizationById")]
        public async Task<ResponseMessage> GetOrganizationById(RequestMessage requestMessage)
        {
            return await this._organizationService.GetOrganizationById(requestMessage);
        }

        [HttpPost("SaveOrganization")]
        public async Task<ResponseMessage> SaveOrganization(RequestMessage requestMessage)
        {
            return await _organizationService.SaveOrganization(requestMessage);
        }
    }
}
