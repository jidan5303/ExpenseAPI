using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.Common.VM;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static CRM.Common.Enums.Enums;

namespace CRM.Services.Services
{

    public class ScheduleTaskService : IScheduleTaskService
    {
        private readonly CRMDbContext _crmDbContext;
        public ScheduleTaskService(CRMDbContext cRMDbContext)
        {
            _crmDbContext = cRMDbContext;
        }
        public async Task<ResponseMessage> GetAllScheduleTask(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                string searchText = requestMessage.RequestObj?.ToString();

                var scheduleTaskQuery = _crmDbContext.VMScheduleTask.Where(x => (string.IsNullOrEmpty(searchText) ? 1 == 1 : x.TaskName.Contains(searchText)));
                List<VMScheduleTask> lstScheduleTask = await scheduleTaskQuery.OrderBy(x => x.ScheduleTaskID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.TotalCount = await scheduleTaskQuery.CountAsync();

                List<int> lstScheduleTaskId = lstScheduleTask.Select(s => s.ScheduleTaskID).Distinct().ToList();
                IQueryable<VMScheduleTaskTag> scheduleTaskTagQuery = _crmDbContext.VMScheduleTaskTag.Where(x => lstScheduleTaskId.Contains(x.ScheduleTaskID));

                List<VMScheduleTaskTag> lstScheduleTaskTag = await scheduleTaskTagQuery.ToListAsync();
                lstScheduleTask.ForEach(scheduleTask =>
                {
                    scheduleTask.lstVMScheduleTaskTag = lstScheduleTaskTag.Where(x => x.ScheduleTaskID == scheduleTask.ScheduleTaskID).ToList();
                });
                responseMessage.ResponseObj = lstScheduleTask;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj ?? "", (int)Enums.ActionType.View, requestMessage?.UserID ?? 0, "GetAllScheduleTask");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllScheduleTask");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllScheduleTaskInitData(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                List<int> lstPriority = Enum.GetValues(typeof(Priority)).Cast<int>().ToList();

                List<ScheduleTask> lstScheduleTask = await _crmDbContext.ScheduleTask.Where(x => x.DueDate != null && x.TaskStatus != (int)ScheduleTaskStatus.Completed).Select(s => new ScheduleTask()
                {
                    ScheduleTaskID = s.ScheduleTaskID,
                    PriorityLevel = s.PriorityLevel,
                    DueDate = s.DueDate
                }).ToListAsync();

                var prioritySum = lstScheduleTask.GroupBy(g => g.PriorityLevel.Value).Select(s =>
                {
                    DateTime today = DateTime.Now;
                    int dueToDay = s.Count(c => c.DueDate.Value.Date == today.Date);
                    int passDay = s.Count(c => c.DueDate.Value.Date < today.Date);
                    int upcomming = s.Count(c => c.DueDate.Value.Date > today.Date);
                    return new ScheduleTaskHeaderVM()
                    {
                        Priority = s.Key,
                        DueToDay = dueToDay,
                        PassDue = passDay,
                        Upcomming = upcomming
                    };
                }).ToList();

                responseMessage.ResponseObj = lstPriority.GroupJoin(prioritySum, p => p, lp => lp.Priority, (p, lp) => new { priority = p, prioritySum = lp })
                    .SelectMany(s => s.prioritySum.DefaultIfEmpty(), (p, lp) => new { p.priority, prioritySum = lp }).Select(s => new ScheduleTaskHeaderVM()
                    {
                        Priority = s.priority,
                        DueToDay = s.prioritySum?.DueToDay.Value,
                        PassDue = s.prioritySum?.PassDue.Value,
                        Upcomming = s.prioritySum?.Upcomming.Value
                    }).ToList();
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj ?? "", (int)Enums.ActionType.View, requestMessage?.UserID ?? 0, "GetAllScheduleTaskInitData");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllScheduleTaskInitData");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllPatientTask(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                int? patientID = Convert.ToInt32(requestMessage?.RequestObj?.ToString() ?? "0");
                List<VMScheduleTask> lstScheduleTask = await _crmDbContext.VMScheduleTask
                  .OrderBy(x => x.ScheduleTaskID)
                  .Where(x => x.RelatedTo == 1 && x.RelatedToID == patientID)
                  .ToListAsync();

                List<int> lstScheduleTaskId = lstScheduleTask.Select(s => s.ScheduleTaskID).Distinct().ToList();
                IQueryable<VMScheduleTaskTag> scheduleTaskTagQuery = _crmDbContext.VMScheduleTaskTag
                  .Where(x => lstScheduleTaskId.Contains(x.ScheduleTaskID));

                List<VMScheduleTaskTag> lstScheduleTaskTag = await scheduleTaskTagQuery.ToListAsync();
                lstScheduleTask.ForEach(scheduleTask =>
                {
                    scheduleTask.lstVMScheduleTaskTag = lstScheduleTaskTag.Where(x => x.ScheduleTaskID == scheduleTask.ScheduleTaskID).ToList();
                });
                responseMessage.ResponseObj = lstScheduleTask;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj ?? "", (int)Enums.ActionType.View, requestMessage?.UserID ?? 0, "GetAllPatientTask");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllPatientTask");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllAideTask(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                int? aideID = Convert.ToInt32(requestMessage?.RequestObj?.ToString());
                List<VMScheduleTask> lstScheduleTask = await _crmDbContext.VMScheduleTask
                  .OrderBy(x => x.ScheduleTaskID)
                  .Where(x => x.RelatedTo == 2 && x.RelatedToID == aideID)
                  .ToListAsync();

                List<int> lstScheduleTaskId = lstScheduleTask.Select(s => s.ScheduleTaskID).Distinct().ToList();
                IQueryable<VMScheduleTaskTag> scheduleTaskTagQuery = _crmDbContext.VMScheduleTaskTag
                  .Where(x => lstScheduleTaskId.Contains(x.ScheduleTaskID));

                List<VMScheduleTaskTag> lstScheduleTaskTag = await scheduleTaskTagQuery.ToListAsync();
                lstScheduleTask.ForEach(scheduleTask =>
                {
                    scheduleTask.lstVMScheduleTaskTag = lstScheduleTaskTag.Where(x => x.ScheduleTaskID == scheduleTask.ScheduleTaskID).ToList();
                });
                responseMessage.ResponseObj = lstScheduleTask;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj ?? "", (int)Enums.ActionType.View, requestMessage?.UserID ?? 0, "GetAllPatientTask");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllPatientTask");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> SaveScheduleTask(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                ScheduleTask objScheduleTask = JsonConvert.DeserializeObject<ScheduleTask>(requestMessage.RequestObj.ToString());

                if (objScheduleTask != null)
                {
                    if (CheckedValidation(objScheduleTask, responseMessage))
                    {
                        if (objScheduleTask.ScheduleTaskID > 0)
                        {
                            ScheduleTask existingScheduleTask = await _crmDbContext.ScheduleTask.AsNoTracking().FirstOrDefaultAsync(x => x.ScheduleTaskID == objScheduleTask.ScheduleTaskID && x.Status == (int)Enums.Status.Active);
                            if (existingScheduleTask != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objScheduleTask.CreatedDate = existingScheduleTask.CreatedDate;
                                objScheduleTask.CreatedBy = existingScheduleTask.CreatedBy;
                                objScheduleTask.UpdatedDate = DateTime.Now;
                                objScheduleTask.UpdatedBy = requestMessage.UserID;
                                _crmDbContext.ScheduleTask.Update(objScheduleTask);

                                List<ScheduleTaskAndTagMapping> lstScheduleTaskAndTagMapping = await _crmDbContext.ScheduleTaskAndTagMapping.AsNoTracking().Where(x => x.ScheduleTaskID == objScheduleTask.ScheduleTaskID && x.Status == (int)Enums.Status.Active).ToListAsync();
                                if (lstScheduleTaskAndTagMapping != null)
                                {

                                    List<int> existingTagIDs = lstScheduleTaskAndTagMapping.Select(s => s.TagID).Distinct().ToList();
                                    var lstNewScheduleTag = objScheduleTask.lstVMScheduleTaskTag.Where(x => !existingTagIDs.Contains(x.TagID))
                                        .Select(s => new ScheduleTaskAndTagMapping()
                                        {
                                            ScheduleTaskID = objScheduleTask.ScheduleTaskID,
                                            TagID = s.TagID,
                                            Status = (int)Enums.Status.Active
                                        })
                                        .ToList();
                                    await _crmDbContext.ScheduleTaskAndTagMapping.AddRangeAsync(lstNewScheduleTag);

                                    //delete exting record but not came in client side 
                                    var newTagsIDs = objScheduleTask?.lstVMScheduleTaskTag?.Select(s => s.TagID).Distinct().ToList();
                                    var lstDeleteTag = lstScheduleTaskAndTagMapping?.Where(x => !newTagsIDs.Contains(x.TagID))
                                        .Select(s => new ScheduleTaskAndTagMapping()
                                        {
                                            ScheduleTaskAndTagID = s.ScheduleTaskAndTagID,
                                            ScheduleTaskID = objScheduleTask.ScheduleTaskID,
                                            TagID = s.TagID,
                                            Status = (int)Enums.Status.Delete
                                        })
                                        .ToList();

                                    _crmDbContext.ScheduleTaskAndTagMapping.UpdateRange(lstDeleteTag);
                                }
                                else
                                {
                                    var lstNewScheduleTag = objScheduleTask.lstVMScheduleTaskTag.Select(s => new ScheduleTaskAndTagMapping()
                                    {
                                        ScheduleTaskID = objScheduleTask.ScheduleTaskID,
                                        TagID = s.TagID,
                                        Status = (int)Enums.Status.Active
                                    }).ToList();
                                    await _crmDbContext.ScheduleTaskAndTagMapping.AddRangeAsync(lstNewScheduleTag);
                                }
                                await _crmDbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            objScheduleTask.Status = (int)Enums.Status.Active;
                            objScheduleTask.CreatedDate = DateTime.Now;
                            objScheduleTask.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.ScheduleTask.AddAsync(objScheduleTask);
                            await _crmDbContext.SaveChangesAsync();

                            if (objScheduleTask.lstVMScheduleTaskTag != null)
                            {
                                var lstNewScheduleTag = objScheduleTask.lstVMScheduleTaskTag.Select(s => new ScheduleTaskAndTagMapping()
                                {
                                    ScheduleTaskID = objScheduleTask.ScheduleTaskID,
                                    TagID = s.TagID,
                                    Status = (int)Enums.Status.Active
                                }).ToList();
                                await _crmDbContext.ScheduleTaskAndTagMapping.AddRangeAsync(lstNewScheduleTag);
                            }
                            await _crmDbContext.SaveChangesAsync();
                        }

                        responseMessage.ResponseObj = objScheduleTask;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage?.RequestObj ?? "", actionType, requestMessage?.UserID ?? 0, "SaveScheduleTask");
                    }
                    else
                    {
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                    }
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.SaveFailed;
                }

            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SavePatient");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }
            return responseMessage;
        }

        //TODO:Some are Incompleted
        public async Task<ResponseMessage> GetScheduleTaskCreateInitData(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                VMScheduleTaskNewInitData objScheduleTask = new VMScheduleTaskNewInitData();
                objScheduleTask.Assignees = await _crmDbContext.SystemUser.Select(s => new SystemUser()
                {
                    SystemUserID = s.SystemUserID,
                    FullName = s.FullName,
                })
                .ToListAsync();

                objScheduleTask.Tags = await _crmDbContext.Tag.Select(s => new Tag()
                {
                    TagID = s.TagID,
                    TagTitle = s.TagTitle,
                    Color = s.Color
                })
                .Distinct()
                .ToListAsync();

                objScheduleTask.TaskStatuses = GetTaskStatuses();

                responseMessage.ResponseObj = objScheduleTask;
            }
            catch (Exception)
            {

                throw;
            }
            return responseMessage;
        }

        //TODO:dummay work
        private static bool CheckedValidation(ScheduleTask objScheduleTask, ResponseMessage responseMessage)
        {
            return true;
        }

        //TODO:dummay work
        public static List<VMScheduleTaskStatus> GetTaskStatuses()
        {
            List<VMScheduleTaskStatus> lstStatus = new List<VMScheduleTaskStatus>();
            lstStatus.Add(new VMScheduleTaskStatus { StatusName = "Leads", ColorCode = "#AAFA90" });
            lstStatus.Add(new VMScheduleTaskStatus { StatusName = "Referral", ColorCode = "#FFE0A5" });
            lstStatus.Add(new VMScheduleTaskStatus { StatusName = "Enrolment", ColorCode = "#A3D5FF" });
            lstStatus.Add(new VMScheduleTaskStatus { StatusName = "Intake", ColorCode = "#FFB39D" });
            return lstStatus;
        }

        public async Task<ResponseMessage> SaveTaskModification(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                VMTaskModification objVMTaskModification = JsonConvert.DeserializeObject<VMTaskModification>(requestMessage.RequestObj.ToString());

                if (objVMTaskModification?.lstScheduleTask.Count() > 0)
                {
                    List<int> lstScheduleTaskId = new List<int>();

                    lstScheduleTaskId = objVMTaskModification.lstScheduleTask.Select(x => x.ScheduleTaskID).Distinct().ToList();

                    var iquaryAbleScheduleTask = _crmDbContext.ScheduleTask.Where(x => lstScheduleTaskId.Contains(x.ScheduleTaskID)).AsQueryable();
                    List<ScheduleTask> lstexistingScheduleTask = iquaryAbleScheduleTask.AsNoTracking().ToList();


                    actionType = (int)Enums.ActionType.Update;
                    if (lstexistingScheduleTask.Count() > 0)
                    {
                        lstexistingScheduleTask.ForEach(x =>
                        {
                            if (objVMTaskModification?.AssigneeID > 0)
                            {
                                x.AssigneeID = objVMTaskModification?.AssigneeID;
                            }
                            DateTime temp;
                            if (DateTime.TryParse(objVMTaskModification.DueDate.ToString(), out temp))
                            {
                                x.DueDate = objVMTaskModification?.DueDate;
                            }
                            
                            //x.RelatedTo = objVMTaskModification?.RelatedTo;
                            x.UpdatedBy = requestMessage.UserID;
                            x.UpdatedDate = DateTime.Now;
                            x.Status = (int)Enums.Status.Active;
                        });
                        _crmDbContext.ScheduleTask.UpdateRange(lstexistingScheduleTask);

                        //TO DO: May be need to user in future __Iliyas
                        //var quaryableTaskAndTagMapping = _crmDbContext.ScheduleTaskAndTagMapping.Where(sm => lstScheduleTaskId.Contains(sm.ScheduleTaskID)).AsQueryable();

                        //List<ScheduleTaskAndTagMapping> lstexinstingScheduleTaskAndTagMapping = quaryableTaskAndTagMapping.AsNoTracking().ToList();

                        //if (lstexinstingScheduleTaskAndTagMapping != null && lstexinstingScheduleTaskAndTagMapping.Count() > 0)
                        //{
                        //    _crmDbContext.ScheduleTaskAndTagMapping.RemoveRange(lstexinstingScheduleTaskAndTagMapping);
                        //}

                        //List<ScheduleTaskAndTagMapping> lstScheduleTaskAndTagMapping = new List<ScheduleTaskAndTagMapping>();

                        //foreach (var item in objVMTaskModification.lstScheduleTask)
                        //{

                        //    foreach (var scheduleTaskAndTagMapping in item.lstVMScheduleTaskTag)
                        //    {
                        //        ScheduleTaskAndTagMapping objScheduleTaskAndTagMapping = new ScheduleTaskAndTagMapping();

                        //        objScheduleTaskAndTagMapping.ScheduleTaskID = item.ScheduleTaskID;
                        //        objScheduleTaskAndTagMapping.TagID = scheduleTaskAndTagMapping.TagID;
                        //        scheduleTaskAndTagMapping.Status = (int)Enums.Status.Active;
                        //        lstScheduleTaskAndTagMapping?.Add(objScheduleTaskAndTagMapping);
                        //    }
                        //}
                        //if (lstScheduleTaskAndTagMapping.Count() > 0)
                        //{
                        //    await _crmDbContext.ScheduleTaskAndTagMapping.AddRangeAsync(lstScheduleTaskAndTagMapping);
                        //}

                        await _crmDbContext.SaveChangesAsync();
                        responseMessage.ResponseObj = objVMTaskModification;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage?.RequestObj ?? "", actionType, requestMessage?.UserID ?? 0, "SaveTaskModification");

                    }
                    else
                    {
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                        responseMessage.Message = MessageConstant.SaveFailed;

                    }
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.SaveFailed;
                }

            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SavePatient");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteScheduleTask(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                List<VMScheduleTask> lstScheduleTask = new List<VMScheduleTask>();
                lstScheduleTask = JsonConvert.DeserializeObject<List<VMScheduleTask>>(requestMessage.RequestObj.ToString());

                if (lstScheduleTask.Count() > 0)
                {
                    List<int> lstScheduleTaskId = new List<int>();

                    lstScheduleTaskId = lstScheduleTask.Select(x => x.ScheduleTaskID).Distinct().ToList();

                    var iquaryAbleScheduleTask = _crmDbContext.ScheduleTask.Where(x => lstScheduleTaskId.Contains(x.ScheduleTaskID)).AsQueryable();
                    List<ScheduleTask> lstexistingScheduleTask = iquaryAbleScheduleTask.AsNoTracking().ToList();


                    actionType = (int)Enums.ActionType.Delete;
                    if (lstexistingScheduleTask.Count() > 0)
                    {
                        lstexistingScheduleTask.ForEach(x =>
                        {
                            x.UpdatedBy = requestMessage.UserID;
                            x.UpdatedDate = DateTime.Now;
                            x.Status = (int)Enums.Status.Delete;
                        });
                        _crmDbContext.ScheduleTask.UpdateRange(lstexistingScheduleTask);

                        await _crmDbContext.SaveChangesAsync();
                        responseMessage.ResponseObj = lstScheduleTask;
                        responseMessage.Message = MessageConstant.DeleteSuccess;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage?.RequestObj ?? "", actionType, requestMessage?.UserID ?? 0, "DeleteScheduleTask");

                    }
                    else
                    {
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                        responseMessage.Message = MessageConstant.DeleteFailed;
                    }
                }
                else
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.SaveFailed;
                }

            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SavePatient");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }
            return responseMessage;
        }

    }

}
