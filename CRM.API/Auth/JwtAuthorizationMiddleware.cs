using CRM.API.Controllers;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRM.API.Auth
{
    public class JwtAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public JwtAuthorizationMiddleware(RequestDelegate next, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext httpContext, IExpensePermissionService permissionService)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                string requestUrl = httpContext.Request.Path.ToString();
                string desiredUrl = null;

                string[] segments = requestUrl.Split('/');

                if (segments.Length == 5)
                {
                    segments = segments.Take(segments.Length - 1).ToArray();
                    desiredUrl = string.Join("/", segments);
                }
                else
                {
                    desiredUrl = requestUrl;
                }
                var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = GetValidationParameters();

                    ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                    httpContext.User = principal;

                    var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;
                    if (roleClaim != null)
                    {
                        var permissionController = new ExpensePermissionController(permissionService);
                        IActionResult actionResult = permissionController.CheckPermission(desiredUrl, roleClaim);
                        if (actionResult != null)
                        {
                            httpContext.Response.StatusCode = StatusCodes.Status200OK;
                        }
                        else
                        {
                            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        }
                    }
                    else
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    }


                    if (httpContext.Response.StatusCode == StatusCodes.Status200OK)
                    {
                        await _next(httpContext);
                        return;
                    }
                    else
                    {
                        await httpContext.Response.WriteAsync("Unauthorized");
                        return;
                    }
                }
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("No token found");
                return;
                //await _next(httpContext);
            }
        }
        private TokenValidationParameters GetValidationParameters()
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        }
    }
}