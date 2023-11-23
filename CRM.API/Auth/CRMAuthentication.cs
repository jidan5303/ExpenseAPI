using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Models;
using CRM.Common.VM;
using CRM.Services;
using CRM.Services.Interface;
using CRM.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.API.Auth
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CRMAuthentication
    {
        private readonly RequestDelegate _next;

        public CRMAuthentication(RequestDelegate next)

        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IAccessTokenService accessTokenService, ISecurityService securityService)
        {

            string url = httpContext.Request.Path;
            if (url?.ToLower() == CommonPath.loginUrl)
            {
                ResponseMessage objResponseMessage = new ResponseMessage();
                RequestMessage objRequestMessage = new RequestMessage();
                using (StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
                {
                    try
                    {
                        var obj = await reader.ReadToEndAsync();
                        objRequestMessage = JsonConvert.DeserializeObject<RequestMessage>(obj);

                        //for getting user info

                        objResponseMessage = await securityService.Login(objRequestMessage);

                        if (objResponseMessage != null && objResponseMessage.ResponseObj != null)
                        {

                            VMLogin objVMLogin = objResponseMessage.ResponseObj as VMLogin;

                            if (objVMLogin != null)
                            {

                                AccessToken objAccessToken = new AccessToken();
                                objAccessToken.SystemUserID = objVMLogin.SystemUserID;
                                objAccessToken.RoleId = objVMLogin.RoleID;

                                //for creating sesssion token
                                AccessToken accessToken = await accessTokenService.Create(objAccessToken);
                                objVMLogin.Token = (accessToken != null) ? accessToken.Token : String.Empty;

                                objVMLogin.SystemUserID = 0;
                                objVMLogin.RoleID = 0;
                                objResponseMessage.ResponseObj = objVMLogin;
                                objResponseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                                var options = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = false
                                };

                                await httpContext.Response.WriteAsJsonAsync(objResponseMessage, options);
                            }
                            else
                            {
                                var options = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = false
                                };

                                await httpContext.Response.WriteAsJsonAsync(objResponseMessage, options);
                            }

                        }
                        else
                        {
                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = false
                            };

                            httpContext.Response.ContentType = "application/json";
                            await httpContext.Response.WriteAsJsonAsync(objResponseMessage, options);
                        }
                    }
                    catch
                    {
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await httpContext.Response.WriteAsJsonAsync(MessageConstant.InternalServerError);
                    }
                }

            }
            else
            {
                await _next(httpContext);
            }


        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CRMAuthenticationExtensions
    {
        public static IApplicationBuilder UseCRMAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CRMAuthentication>();
        }
    }
}
