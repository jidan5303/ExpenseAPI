using CRM.API.Controllers;
using CRM.Common.DTO;
using CRM.Common.Models;
using CRM.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace CRM.API.Auth
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public JwtAuthenticationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext httpContext, IAuthService authService)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                string requestUrl = httpContext.Request.Path.ToString();

                if (requestUrl.StartsWith("/api/AuthController/Login"))
                {
                    using (var streamReader = new StreamReader(httpContext.Request.Body))
                    {
                        var requestBody = await streamReader.ReadToEndAsync();
                        var data = JsonConvert.DeserializeObject<RequestMessage>(requestBody);
                        UserLogin userLogin = JsonConvert.DeserializeObject<UserLogin>(data.RequestObj.ToString());


                        var authController = new AuthController(_configuration, authService);
                        IActionResult actionResult = authController.Login(userLogin);
                        IActionResult getRole = authController.GetUserRole(userLogin.username);

                        if (actionResult is OkObjectResult okResult)
                        {
                            if (getRole is OkObjectResult okRole)
                            {
                                var token = okResult.Value.ToString();
                                var role = okRole.Value.ToString();

                                httpContext.Response.Headers.Add("Authorization", "Bearer " + token);


                                var responseObj = new { token, userLogin.username, role };

                                var responseBody = JsonConvert.SerializeObject(responseObj);

                                httpContext.Response.ContentType = "application/json";
                                await httpContext.Response.WriteAsync(responseBody);
                                return;
                            }
                        }
                    }
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Invalid credentials");
                    return;
                }
                await _next(httpContext);
            }
        }
    }
}