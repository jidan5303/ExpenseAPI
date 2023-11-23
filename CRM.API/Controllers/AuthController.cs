using CRM.Common.Models;
using CRM.Services.Interface;
using CRM.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRM.API.Controllers
{
    [Route("api/AuthController")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            var user = _authService.Authenticate(userLogin);
            if (user != null)
            {
                var session = _authService.CheckSession(user);
                var token = Generate(user);
                var addSession = _authService.CreateSession(token, user);
                return Ok(token);
            }
            return NotFound("User not found");
        }

        [HttpPost]
        [Route("GetUserRole")]
        public IActionResult GetUserRole(string name)
        {
            var userRole = _authService.getRole(name);
            if (userRole != null)
            {
                return Ok(userRole);
            }
            return BadRequest("Not Found");
        }
        private string Generate(ExpenseUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
