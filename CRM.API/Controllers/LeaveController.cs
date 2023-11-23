using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models; 
using CRM.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly DatavancedDbContext _context;

        public LeaveController(DatavancedDbContext context)
        {
            _context = context;
        }

        [HttpPost("getLeaves")]
        public async Task<ResponseMessage> GetLeaves(RequestMessage request)
        {
            var response = new ResponseMessage();
            try
            {
                var skip = 0;
                var take = 20;
                response.ResponseCode = (int)Enums.ResponseCode.Success;
                response.ResponseObj = _context.Leaves.Skip(skip).Take(take);

            }
            catch (Exception e)
            {
                response.Message = ExceptionHelper.ProcessException(e, (int)Enums.ActionType.View, request!.UserID, JsonConvert.SerializeObject(request.RequestObj), "GetAllActions");
                response.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return response;
        }


        [HttpPost("getLeaveId")]
        public async Task<ResponseMessage> GetLeaveId(RequestMessage request)
        {
            var response = new ResponseMessage();
            try
            {
                var skip = 0;
                var take = 20;
                response.ResponseCode = (int)Enums.ResponseCode.Success;
                response.ResponseObj = _context.Leaves.Skip(skip).Take(take);

            }
            catch (Exception e)
            {
                response.Message = ExceptionHelper.ProcessException(e, (int)Enums.ActionType.View, request!.UserID, JsonConvert.SerializeObject(request.RequestObj), "GetAllActions");
                response.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return response;
        }

        [HttpPost("save")]
        public async Task<ResponseMessage> Save(RequestMessage request)
        {
            var response = new ResponseMessage();
            try
            {

                var leave = JsonConvert.DeserializeObject<Leave>(request.RequestObj.ToString()!);
                if (leave != null)
                {
                    if (leave.Id > 0)
                        _context.Leaves.Update(leave);
                    else
                        _context.Leaves.Add(leave);

                    await _context.SaveChangesAsync();
                    response.ResponseCode = (int)Enums.ResponseCode.Success;
                    response.ResponseObj = leave;
                    response.Message = "Saved Successfully";
                }
                else
                {
                    response.ResponseCode = (int)Enums.ResponseCode.Failed;
                    response.ResponseObj = leave;
                    response.Message = "Failed to save data";

                }
            }
            catch (Exception e)
            {
                response.Message = ExceptionHelper.ProcessException(e, (int)Enums.ActionType.View, request!.UserID, JsonConvert.SerializeObject(request.RequestObj), "GetAllActions");
                response.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return response;
        }



        [HttpPost("delete")]
        public async Task<ResponseMessage> Delete(RequestMessage request)
        {
            var response = new ResponseMessage();
            try
            {

                var leave = JsonConvert.DeserializeObject<Leave>(request.RequestObj.ToString()!);
                var existingLeave = _context.Leaves.FirstOrDefault(x => x.Id == leave!.Id);
                if (existingLeave != null)
                {

                    _context.Leaves.Remove(existingLeave);
                    await _context.SaveChangesAsync();
                    response.ResponseCode = (int)Enums.ResponseCode.Success;
                    response.Message = "Removed Successfully";
                }
                else
                {
                    response.ResponseCode = (int)Enums.ResponseCode.Failed;
                    response.Message = "Failed to remove leave";

                }
            }
            catch (Exception e)
            {
                response.Message = ExceptionHelper.ProcessException(e, (int)Enums.ActionType.View, request!.UserID, JsonConvert.SerializeObject(request.RequestObj), "GetAllActions");
                response.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return response;
        }


    }
}

