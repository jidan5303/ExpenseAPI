using CRM.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/ExpensePermission")]
    [ApiController]
    public class ExpensePermissionController : ControllerBase
    {
        private readonly IExpensePermissionService _permissionService;
        public ExpensePermissionController(IExpensePermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        [Route("CheckPermission")]
        public IActionResult CheckPermission(string url, string role)
        {
            var data = _permissionService.Validate(url, role);
            if (data != null)
            {
                return Ok(data);
            }
            return null;
        }
    }
}
