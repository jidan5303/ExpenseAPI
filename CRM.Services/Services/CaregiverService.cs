using CRM.Common.Constants;
using CRM.Common.DTO;
using CRM.Common.Enums;
using CRM.Common.Helper;
using CRM.Common.Models;
using CRM.Common.QueryHelper;
using CRM.Common.VM;
using CRM.DataAccess;
using CRM.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services
{
#pragma warning disable CS8600
#pragma warning disable CS8603
    public class CaregiverService : ICaregiverService
    {
        private readonly CRMDbContext _crmDbContext;
        private readonly BuildDynamicFilter _buildDynamicFilter;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public CaregiverService(CRMDbContext ctx, IServiceScopeFactory serviceScopeFactory, BuildDynamicFilter buildDynamicFilter)
        {
            this._crmDbContext = ctx;
            this._serviceScopeFactory = serviceScopeFactory;
            this._buildDynamicFilter = buildDynamicFilter;

        }

        /// <summary>
        /// Get all Caregiver
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCaregiver(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Caregiver> lstCaregiver = new List<Caregiver>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstCaregiver = await _crmDbContext.Caregiver.OrderBy(x => x.CaregiverID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstCaregiver;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCaregiver");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCaregiver");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get all Caregiver
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCaregiverList(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                VMCaregivers objVMCaregiver = new VMCaregivers();
                List<VMCaregiver> lstCaregiver = new List<VMCaregiver>();
                int departmentID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                int totalCount = 0;
                int totalSkip = 0;

                totalCount = await _crmDbContext.Caregiver.CountAsync();

                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;
                responseMessage.TotalCount = await _crmDbContext.VMCaregiver.CountAsync();
                lstCaregiver = await _crmDbContext.VMCaregiver.OrderBy(x => x.CaregiverID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();

                if (lstCaregiver.Count > 0)
                {
                    var lstCaregiverTag = _crmDbContext.VMCaregiverTag.AsQueryable();
                    List<VMCaregiverTag> lstVMCaregiverTag = lstCaregiverTag.ToList();


                    foreach (VMCaregiver vmCaregiver in lstCaregiver)
                    {
                        vmCaregiver.EmploymentStatusName = Enum.GetName(typeof(Enums.EmploymentStatus), vmCaregiver.EmploymentStatus);
                        vmCaregiver.lstTag = GetTagList(vmCaregiver.CaregiverID, lstVMCaregiverTag);
                        //todo ilias it will be come from HHAEX
                        vmCaregiver.LeadSource = "HHAEX";
                        vmCaregiver.OnboardingCoordinator = "Kaden Massey";
                        vmCaregiver.CoordinationOnboardingCoordinator = "Nathen Dawson";
                        vmCaregiver.Assignee = "Nathen";
                        vmCaregiver.AssignedTo = "Atticus Lawrence";
                        vmCaregiver.InPlan = "United Ins.";
                        vmCaregiver.County = "Az";
                        vmCaregiver.RateOfPay = 15;

                        //....
                    }
                    int[] lstCaregiverIds = lstCaregiver.Select(x => x.CaregiverID).Distinct().ToArray();
                    objVMCaregiver.lstCaregiverHistory = await _crmDbContext.VMCaregiverLastChange.Where(x => lstCaregiverIds.Contains(x.CaregiverID)).ToListAsync();
                }

                responseMessage.TotalCount = totalCount;
                responseMessage.ResponseObj = objVMCaregiver;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCaregiverWithChange");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCaregiverWithChange");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get all Caregiver list by department id.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCaregiverListByDepartmentID(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                VMCaregivers objVMCaregiver = new VMCaregivers();
                List<VMCaregiver> lstCaregiver = new List<VMCaregiver>();
                int departmentID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;
                responseMessage.TotalCount = await _crmDbContext.VMCaregiver.CountAsync(x => x.DepartmentID == departmentID);
                lstCaregiver = await _crmDbContext.VMCaregiver.Where(c => c.DepartmentID == departmentID).OrderBy(x => x.CaregiverID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();

                if (lstCaregiver.Count > 0)
                {
                    var lstCaregiverTag = _crmDbContext.VMCaregiverTag.AsQueryable();
                    List<VMCaregiverTag> lstVMCaregiverTag = lstCaregiverTag.ToList();



                    foreach (VMCaregiver vmCaregiver in lstCaregiver)
                    {
                        vmCaregiver.EmploymentStatusName = Enum.GetName(typeof(Enums.EmploymentStatus), vmCaregiver.EmploymentStatus);
                        vmCaregiver.lstTag = GetTagList(vmCaregiver.CaregiverID, lstVMCaregiverTag);
                        //todo ilias it will be come from HHAEX
                        vmCaregiver.LeadSource = "HHAEX";
                        vmCaregiver.OnboardingCoordinator = "Kaden Massey";
                        vmCaregiver.CoordinationOnboardingCoordinator = "Nathen Dawson";
                        vmCaregiver.Assignee = "Nathen";
                        vmCaregiver.AssignedTo = "Atticus Lawrence";
                        vmCaregiver.InPlan = "United Ins.";
                        vmCaregiver.County = "Az";
                        vmCaregiver.RateOfPay = 15;
                        //...
                    }
                    objVMCaregiver.lstCaregiver = lstCaregiver;
                    int[] lstCaregiverIds = lstCaregiver.Select(x => x.CaregiverID).Distinct().ToArray();
                    objVMCaregiver.lstCaregiverHistory = await _crmDbContext.VMCaregiverLastChange.Where(x => lstCaregiverIds.Contains(x.CaregiverID)).ToListAsync();

                }
                else
                {
                    responseMessage.TotalCount = 0;
                }


                responseMessage.ResponseObj = objVMCaregiver;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCaregiverWithChange");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCaregiverWithChange");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }


        /// <summary>
        /// Get all Caregiver list by All Search.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCaregiverWithAllSearch(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                VMCaregivers objVMCaregiver = new VMCaregivers();
                List<VMCaregiver> lstCaregiver = new List<VMCaregiver>();

                int totalCount = 0;

                int departmentID = 0;
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                string queryString = "Select * from [dbo].[GetAllCaregiver] where";

                QueryObject objQueryObject = JsonConvert.DeserializeObject<QueryObject>(requestMessage.RequestObj.ToString());
                int.TryParse(objQueryObject.BasicSearch.ToString(), out departmentID);


                string whereQuery = " [DepartmentID] = " + departmentID;

                //For global search..
                if (!string.IsNullOrEmpty(objQueryObject.SearchText) && objQueryObject?.FilterModel?.Count == 0)
                {
                    whereQuery += @" and (LTRIM(RTRIM([FirstName])) + LTRIM(RTRIM([LastName]))) like '%" + objQueryObject.SearchText.Replace(" ", "") + "%' OR"
                        + "[PhoneNumber] like '%" + objQueryObject.SearchText + "%' OR" + "[Email] like '%" + objQueryObject.SearchText + "%' OR"
                        + "[City] like '%" + objQueryObject.SearchText + "%' OR" + "[Zip] like '%" + objQueryObject.SearchText + "%'";
                }

                //for generate filter.
                if (objQueryObject?.FilterModel != null && objQueryObject.FilterModel.Count > 0)
                {
                    whereQuery += _buildDynamicFilter.GetFilterCluse(objQueryObject.FilterModel);

                }

                //need always  call before this GenerateSortQuery for total count.
                totalCount = await _crmDbContext.VMPatient.FromSqlRaw("SELECT [CaregiverID] FROM [dbo].[GetAllCaregiver] where" + whereQuery).CountAsync();

                //for sort
                if (objQueryObject?.SortModel?.Count > 0)
                {
                    whereQuery += _buildDynamicFilter.GenerateSortQuery(objQueryObject?.SortModel);
                }

                queryString += whereQuery;

                //check condition
                if (objQueryObject?.SortModel?.Count > 0)
                {
                    queryString += $"  OFFSET {totalSkip} ROWS FETCH NEXT {requestMessage.PageRecordSize} ROWS ONLY";
                }
                else
                {
                    queryString += $" ORDER BY [CaregiverID] DESC OFFSET {totalSkip} ROWS FETCH NEXT {requestMessage.PageRecordSize} ROWS ONLY";
                }



                lstCaregiver = await _crmDbContext.VMCaregiver.FromSqlRaw(queryString).ToListAsync();

                if (lstCaregiver.Count > 0)
                {
                    int[] lstCaregiverIds = lstCaregiver.Select(x => x.CaregiverID).Distinct().ToArray();

                    var lstCaregiverTag = _crmDbContext.VMCaregiverTag.Where(x => lstCaregiverIds.Contains(x.CaregiverID)).AsQueryable();
                    List<VMCaregiverTag> lstVMCaregiverTag = lstCaregiverTag.ToList();

                    List<int?> lstStatusIds = lstCaregiver.Select(c => c.EmploymentStatus).Distinct().ToList();

                    List<Status> lstStatus = new List<Status>();
                    if (lstStatusIds.Count() > 0)
                    {
                        var statusList = _crmDbContext.Status.Where(x => lstStatusIds.Contains(x.StatusID)).AsQueryable();
                        lstStatus = statusList.ToList();
                    }

                    foreach (VMCaregiver vmCaregiver in lstCaregiver)
                    {
                        //   vmCaregiver.EmploymentStatusName = Enum.GetName(typeof(Enums.EmploymentStatus), vmCaregiver.EmploymentStatus);

                        Status objStatus = lstStatus.FirstOrDefault(x => x.StatusID == vmCaregiver.EmploymentStatus);
                        vmCaregiver.EmploymentStatusName = (objStatus != null) ? objStatus.StatusName : "";
                        vmCaregiver.Color = (objStatus != null) ? objStatus.Color : "";
                        vmCaregiver.lstTag = GetTagList(vmCaregiver.CaregiverID, lstVMCaregiverTag);
                        //todo ilias it will be come from HHAEX
                        vmCaregiver.LeadSource = "HHAEX";
                        vmCaregiver.OnboardingCoordinator = "Kaden Massey";
                        vmCaregiver.CoordinationOnboardingCoordinator = "Nathen Dawson";
                        vmCaregiver.Assignee = "Nathen";
                        vmCaregiver.AssignedTo = "Atticus Lawrence";
                        vmCaregiver.InPlan = "United Ins.";
                        vmCaregiver.County = "Az";
                        vmCaregiver.RateOfPay = 15;
                        //...
                    }

                    objVMCaregiver.lstCaregiver = lstCaregiver;
                    var lstlstCaregiverHistoryAsQuaryable = _crmDbContext.VMCaregiverLastChange.Where(x => lstCaregiverIds.Contains(x.CaregiverID)).AsQueryable();
                    objVMCaregiver.lstCaregiverHistory = lstlstCaregiverHistoryAsQuaryable.ToList();
                    responseMessage.TotalCount = totalCount;
                }
                else
                {
                    responseMessage.TotalCount = 0;
                }

                responseMessage.ResponseObj = objVMCaregiver;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCaregiverWithChange");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCaregiverWithChange");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Get all Caregiver
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCaregiverInitData(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                VMCaregiverInit objVMCaregiver = new VMCaregiverInit();

                objVMCaregiver.lstOrganization = await GetOrganizationWithValue();
                objVMCaregiver.lstAssignTo = await GetAllAssignewithValue(requestMessage.UserID);
                objVMCaregiver.lstSource = GetAllSource();
                objVMCaregiver.lstTags = GetAllTagsValue();
                objVMCaregiver.lstHeaderStatus = await GetAllStatusWithValue();
                objVMCaregiver.lstStatus = await GetAllStatus();
                responseMessage.ResponseObj = objVMCaregiver;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCaregiverWithChange");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCaregiverWithChange");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return await Task.FromResult(responseMessage);
        }

        /// <summary>
        /// for get all status.
        /// </summary>
        /// <returns></returns>
        private async Task<List<Status>> GetAllStatus()
        {
            return await _crmDbContext.Status.OrderBy(x => x.StatusID).ToListAsync();
        }

        /// <summary>
        /// for getting all status with value.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<List<CaregiverStatus>> GetAllStatusWithValue()
        {
            List<CaregiverStatus> lstCaregiverStatus = new List<CaregiverStatus>();
            lstCaregiverStatus = await _crmDbContext.CaregiverStatus.ToListAsync();
            return lstCaregiverStatus;
        }


        /// <summary>
        /// for get all tags with value/
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private List<Tags> GetAllTagsValue()
        {
            List<Tags> lstTags = new List<Tags>();

            List<CRM.Common.Models.Tag> lstTag = new List<CRM.Common.Models.Tag>();

            var lstCaregiverTag = _crmDbContext.VMCaregiverTag.AsQueryable();

            List<int> lstCaregiverTagId = lstCaregiverTag.Select(t => t.TagID).Distinct().ToList();//for get distinct tag id

            if (lstCaregiverTagId.Count > 0)
            {
                List<VMCaregiverTag> lstVMCaregiverTag = lstCaregiverTag.ToList();
                var lstCareTags = _crmDbContext.Tag.AsQueryable();

                lstTag = lstCareTags.Where(tg => lstCaregiverTagId.Contains(tg.TagID)).ToList();

                foreach (Tag tag in lstTag)
                {
                    Tags objTags = new Tags();

                    objTags.TagTitle = tag.TagTitle;
                    objTags.Color = tag.Color;
                    objTags.TotalTagsValue = lstVMCaregiverTag.Count(x => x.TagID == tag.TagID);
                    lstTags.Add(objTags);
                }

            }

            return lstTags;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private List<Source> GetAllSource()
        {
            //in future it will come from DB.
            List<Source> lstSource = new List<Source>();

            lstSource.Add(new Source { SourceName = "Website", TotalSourceValue = 23 });
            lstSource.Add(new Source { SourceName = "Eugene", TotalSourceValue = 23 });
            lstSource.Add(new Source { SourceName = "Miller", TotalSourceValue = 25 });
            lstSource.Add(new Source { SourceName = "Eugena", TotalSourceValue = 23 });
            lstSource.Add(new Source { SourceName = "Brandon", TotalSourceValue = 25 });
            lstSource.Add(new Source { SourceName = "Eugenu", TotalSourceValue = 23 });
            lstSource.Add(new Source { SourceName = "Eugena1", TotalSourceValue = 23 });
            lstSource.Add(new Source { SourceName = "Brandon1", TotalSourceValue = 25 });
            lstSource.Add(new Source { SourceName = "Eugenu1", TotalSourceValue = 23 });

            return lstSource;

        }

        /// <summary>
        /// for gat all assign list with total
        /// </summary>
        /// <returns></returns>
        private async Task<List<CaregiverAssignTo>> GetAllAssignewithValue(int userid)
        {
            //in future it will come from DB.
            List<CaregiverAssignTo> lstCaregiverAssignTo = new List<CaregiverAssignTo>();
            lstCaregiverAssignTo = await _crmDbContext.CaregiverAssignTo.ToListAsync();

            if (lstCaregiverAssignTo.Count > 0)
            {
                foreach (CaregiverAssignTo item in lstCaregiverAssignTo)
                {
                    if (item.AssignID == userid)
                    {
                        item.AssignName = "Me";
                    }

                }

            }
            return lstCaregiverAssignTo;
        }


        /// <summary>
        /// method for get CaregiverOrganization list with value.
        /// </summary>
        /// <returns></returns>
        private async Task<List<CaregiverOrganization>> GetOrganizationWithValue()
        {
            List<CaregiverOrganization> lstCaregiverOrganization = new List<CaregiverOrganization>();
            lstCaregiverOrganization = await _crmDbContext.CaregiverOrganization.ToListAsync();
            return lstCaregiverOrganization;
        }


        /// <summary>
        /// method for getting list of tag.
        /// </summary>
        /// <param name="caregiverID"></param>
        /// <returns></returns>
        private List<Tags> GetTagList(int caregiverID, List<VMCaregiverTag> lstVMCaregiverTag)
        {
            List<Tags> lstTags = new List<Tags>();

            foreach (VMCaregiverTag item in lstVMCaregiverTag.Where(x => x.CaregiverID == caregiverID).ToList())
            {
                Tags objTags = new Tags();
                objTags.TagID= item.TagID;
                objTags.TagTitle = item.TagTitle;
                objTags.Color = item.Color;
                lstTags.Add(objTags);
            }

            return lstTags;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessage> GetCaregiverById(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Caregiver objCaregiver = new Caregiver();
                int CaregiverID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

                objCaregiver = await _crmDbContext.Caregiver.AsNoTracking().FirstOrDefaultAsync(x => x.CaregiverID == CaregiverID && x.Status == (int)Enums.Status.Active);
                responseMessage.ResponseObj = objCaregiver;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetCaregiverById");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetCaregiverById");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        /// <summary>
        /// Save and update Caregiver
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> SaveCaregiver(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {

                Caregiver objCaregiver = JsonConvert.DeserializeObject<Caregiver>(requestMessage?.RequestObj.ToString());

                if (objCaregiver != null)
                {
                    if (CheckedValidation(objCaregiver, responseMessage))
                    {
                        if (objCaregiver.CaregiverID > 0)
                        {
                            Caregiver objExistingCaregiver = await this._crmDbContext.Caregiver.AsNoTracking().FirstOrDefaultAsync(x => x.CaregiverID == objCaregiver.CaregiverID && x.Status == (int)Enums.Status.Active);
                            if (objExistingCaregiver != null)
                            {
                                actionType = (int)Enums.ActionType.Update;
                                objCaregiver.CreatedDate = objExistingCaregiver.CreatedDate;
                                objCaregiver.CreatedBy = objExistingCaregiver.CreatedBy;
                                objCaregiver.UpdatedDate = DateTime.Now;
                                objCaregiver.UpdatedBy = requestMessage.UserID;
                                objCaregiver.FullName = objCaregiver.FirstName + " " + objCaregiver.LastName;
                                _crmDbContext.Caregiver.Update(objCaregiver);


                                List<CaregiverAndTagMapping> lstexisCaregiverTag = await _crmDbContext.CaregiverAndTagMapping.Where(x => x.CaregiverID == objCaregiver.CaregiverID).AsNoTracking().ToListAsync();
                                if (lstexisCaregiverTag != null && lstexisCaregiverTag.Count() > 0)
                                {
                                    _crmDbContext.CaregiverAndTagMapping.RemoveRange(lstexisCaregiverTag);
                                }

                                List<CaregiverAndTagMapping> lstCaregiverTagMapping = new List<CaregiverAndTagMapping>();

                               lstCaregiverTagMapping = objCaregiver?.lstTag?.Select(s => new CaregiverAndTagMapping()
                                {
                                    CaregiverID = objCaregiver.CaregiverID,
                                    TagID = s.TagID,
                                    Status = (int)Enums.Status.Active
                                }).ToList();


                                await _crmDbContext.CaregiverAndTagMapping.AddRangeAsync(lstCaregiverTagMapping);


                                //For insert caregiver history...
                                InsertCaregiverHistory(objCaregiver, objExistingCaregiver, requestMessage.UserID);

                                await _crmDbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            objCaregiver.FullName = objCaregiver.FirstName + " " + objCaregiver.LastName;
                            objCaregiver.Status = (int)Enums.Status.Active;
                            objCaregiver.CreatedDate = DateTime.Now;
                            objCaregiver.CreatedBy = requestMessage.UserID;
                            await _crmDbContext.Caregiver.AddAsync(objCaregiver);
                            await _crmDbContext.SaveChangesAsync();

                            if (objCaregiver.lstTag != null)
                            {
                                var lstNewCaregiverTag = objCaregiver.lstTag.Select(s => new CaregiverAndTagMapping()
                                {
                                    CaregiverID = objCaregiver.CaregiverID,
                                    TagID = s.TagID,
                                    Status = (int)Enums.Status.Active
                                }).ToList();
                                await _crmDbContext.CaregiverAndTagMapping.AddRangeAsync(lstNewCaregiverTag);
                                await _crmDbContext.SaveChangesAsync();
                            }

                        }
                        

                        responseMessage.ResponseObj = objCaregiver;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SaveCaregiver");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SaveCaregiver");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }


            return responseMessage;
        }

        /// <summary>
        /// method for get caregiver lsit from dropdown.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetCaregiverForDropdown(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                string searchText = requestMessage.RequestObj?.ToString();
                List<VMCaregiver> lstCaregiver = await _crmDbContext.VMCaregiver
                    .Where(x => x.Status == (int)Enums.Status.Active && 
                    ( ( string.IsNullOrEmpty(searchText) ? 1 == 1 : x.FirstName.Contains(searchText) || x.LastName.Contains(searchText) || x.DepartmentName.Contains(searchText) ) ))
                    .Select(s => new VMCaregiver()
                    {
                        CaregiverID = s.CaregiverID,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Gender = s.Gender,
                        DepartmentID = s.DepartmentID,
                        DepartmentName = s.DepartmentName
                    })
                    .Take(10)
                    .ToListAsync();

                responseMessage.ResponseObj = lstCaregiver;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetCaregiverForDropdown");
            }
            catch (Exception ex)
            {
                //Process excetion, Development mode show real exception and production mode will show custom exception.
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetCaregiverForDropdown");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        /// <summary>
        /// insert caregiver history 
        /// </summary>
        /// <param name="objCaregiver"></param>
        /// <param name="existingCaregiver"></param>
        /// <param name="userId"></param>
        private void InsertCaregiverHistory(Caregiver objCaregiver, Caregiver existingCaregiver, int userId)
        {
            CaregiverSequence objCaregiverSequence = new CaregiverSequence();
            List<CaregiverHistoryNew> lstCaregiverHistoryNew = new List<CaregiverHistoryNew>();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<CRMDbContext>();
                using var transaction = db.Database.BeginTransaction();
                try
                {

                    Dictionary<string, string> lsFinalValueChange = new Dictionary<string, string>();
                    Dictionary<string, string> lstOldValue = new Dictionary<string, string>();
                    lsFinalValueChange = GetAllChangeValue(objCaregiver, existingCaregiver, out lstOldValue); // get all change value..


                    objCaregiverSequence = db.CaregiverSequence.OrderBy(x => x.CaregiverSequenceID).LastOrDefault();
                    int seqNo = (objCaregiverSequence != null && objCaregiverSequence.CaregiverSequenceNo > 0) ? objCaregiverSequence.CaregiverSequenceNo + 1 : 1;


                    foreach (var item in lsFinalValueChange)
                    {
                        CaregiverHistoryNew objCaregiverHistoryNew = new CaregiverHistoryNew();
                        objCaregiverHistoryNew.CaregiverID = objCaregiver.CaregiverID;
                        objCaregiverHistoryNew.SequenceNo = seqNo;
                        objCaregiverHistoryNew.ColumnName = item.Key;
                        objCaregiverHistoryNew.OldValue = lstOldValue[item.Key];
                        objCaregiverHistoryNew.NewValue = item.Value;
                        objCaregiverHistoryNew.CreatedDate = DateTime.Now;
                        objCaregiverHistoryNew.CreatedBy = userId;

                        lstCaregiverHistoryNew.Add(objCaregiverHistoryNew);

                        db.CaregiverHistoryNew.Add(objCaregiverHistoryNew);
                    }
                    if (lsFinalValueChange.Count > 0)
                    {
                        objCaregiverSequence = new CaregiverSequence();
                        objCaregiverSequence.CaregiverSequenceNo = seqNo;
                        db.CaregiverSequence.Add(objCaregiverSequence);
                    }

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.Insert, userId, JsonConvert.SerializeObject(lstCaregiverHistoryNew), "InsertCaregiverHistory");
                }
            }

        }

        /// <summary>
        /// Get All Caregiver Giver History By DepartmentID
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCaregiverGiverHistoryByDepartmentID(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                List<VMCaregiverGiverHistory> lstVMCaregiverGiverHistory = new List<VMCaregiverGiverHistory>();

                int totalCount = 0;

                int departmentID = 0;
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;
                DateTime formDate = DateTime.Now.AddDays(-7);

                QueryObject objQueryObject = JsonConvert.DeserializeObject<QueryObject>(requestMessage.RequestObj.ToString());
                _ = int.TryParse(objQueryObject?.BasicSearch?.ToString(), out departmentID);

                if (objQueryObject?.LstColumnName != null && objQueryObject?.LstColumnName[0] == "FullName")
                {
                    objQueryObject.LstColumnName = new List<string>();
                    objQueryObject?.LstColumnName.Add("FirstName");
                    objQueryObject?.LstColumnName.Add("LastName");
                }

                //for getting Iqueryable data.
                IQueryable<VMCaregiverGiverHistory> query = _crmDbContext.VMCaregiverGiverHistory
                    .Where(x => x.DepartmentID == departmentID &&
                    (objQueryObject.FromDate == null ? formDate < x.ExecutionDate.Date : objQueryObject.FromDate.Value.Date < x.ExecutionDate.Date && x.ExecutionDate.Date <= objQueryObject.ToDate.Value.Date)
                    && (objQueryObject.LstColumnName == null || objQueryObject.LstColumnName.Contains(x.ColumnName)));

                //for total count
                totalCount = await query.CountAsync();

                //for getting list
                lstVMCaregiverGiverHistory = await query.OrderBy(x => x.CaregiverID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();


                responseMessage.ResponseObj = lstVMCaregiverGiverHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCaregiverWithChange");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCaregiverWithChange");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }


        /// <summary>
        /// Revert  Caregiver
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseMessage> RevertCaregiver(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            int actionType = (int)Enums.ActionType.Insert;
            try
            {
                VMCaregiverGiverHistory objVMCaregiverGiverHistory = JsonConvert.DeserializeObject<VMCaregiverGiverHistory>(requestMessage?.RequestObj.ToString());

                Caregiver objExistingCaregiver = new Caregiver();
                CaregiverSequence objCaregiverSequence = new CaregiverSequence();

                if (objVMCaregiverGiverHistory != null)
                {
                    if (CheckedCaregiverGiverHistoryValidation(objVMCaregiverGiverHistory, responseMessage))
                    {

                        if (objVMCaregiverGiverHistory.CaregiverID > 0)
                        {
                            objExistingCaregiver = await _crmDbContext.Caregiver.AsNoTracking().FirstOrDefaultAsync(x => x.CaregiverID == objVMCaregiverGiverHistory.CaregiverID && x.Status == (int)Enums.Status.Active);
                            if (objExistingCaregiver != null)
                            {
                                objCaregiverSequence = _crmDbContext.CaregiverSequence.OrderBy(x => x.CaregiverSequenceID).AsNoTracking().LastOrDefault();
                                int seqNo = (objCaregiverSequence != null && objCaregiverSequence.CaregiverSequenceNo > 0) ? objCaregiverSequence.CaregiverSequenceNo + 1 : 1;

                                //mapping data with caregiver.
                                MappedColumnData(objExistingCaregiver, objVMCaregiverGiverHistory);

                                actionType = (int)Enums.ActionType.Update;
                                objExistingCaregiver.UpdatedDate = DateTime.Now;
                                objExistingCaregiver.UpdatedBy = requestMessage.UserID;

                                _crmDbContext.Caregiver.Update(objExistingCaregiver);

                                //mapp data..
                                CaregiverHistoryNew objCaregiverHistoryNew = new CaregiverHistoryNew();
                                objCaregiverHistoryNew.CaregiverID = objVMCaregiverGiverHistory.CaregiverID;
                                objCaregiverHistoryNew.SequenceNo = seqNo;
                                objCaregiverHistoryNew.ColumnName = objVMCaregiverGiverHistory.ColumnName;
                                objCaregiverHistoryNew.OldValue = objVMCaregiverGiverHistory.NewValue;
                                objCaregiverHistoryNew.NewValue = objVMCaregiverGiverHistory.OldValue;
                                objCaregiverHistoryNew.CreatedDate = DateTime.Now;
                                objCaregiverHistoryNew.CreatedBy = requestMessage.UserID;

                                await _crmDbContext.CaregiverHistoryNew.AddAsync(objCaregiverHistoryNew);
                                await _crmDbContext.SaveChangesAsync();

                            }
                        }


                        responseMessage.ResponseObj = objExistingCaregiver;
                        responseMessage.Message = MessageConstant.SavedSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

                        //Log write
                        LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "RevertCaregiver");
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
                responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "RevertCaregiver");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

            }

            return responseMessage;
        }


        /// <summary>
        /// Get all status
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllStatus(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Status> lstStatus = new List<Status>();
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

                lstStatus = await _crmDbContext.Status.OrderBy(x => x.StatusID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();
                responseMessage.ResponseObj = lstStatus;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllStatus");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllStatus");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }


        /// <summary>
        /// method for mapping data.
        /// </summary>
        /// <param name="objExistingCaregiver"></param>
        /// <param name="objVMCaregiverGiverHistory"></param>
        private void MappedColumnData(Caregiver objExistingCaregiver, VMCaregiverGiverHistory objVMCaregiverGiverHistory)
        {

            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(Caregiver).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                string colname = propertyInfo.Name;
                if (colname == objVMCaregiverGiverHistory.ColumnName)
                {
                    propertyInfo.SetValue(objExistingCaregiver, objVMCaregiverGiverHistory.OldValue);
                    break;

                }
            }
        }

        private bool CheckedCaregiverGiverHistoryValidation(VMCaregiverGiverHistory objVMCaregiverGiverHistory, ResponseMessage responseMessage)
        {
            return true;
        }

        /// <summary>
        /// Get old and new value list.
        /// </summary>
        /// <param name="objCaregiver"></param>
        /// <param name="existingCaregiver"></param>
        /// <param name="lstOldValue"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetAllChangeValue(Caregiver objCaregiver, Caregiver existingCaregiver, out Dictionary<string, string> lstOldValue)
        {
            Dictionary<string, string> lstNewValue = new Dictionary<string, string>();
            lstOldValue = new Dictionary<string, string>();
            Dictionary<string, string> lsFinalValueChange = new Dictionary<string, string>();

            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(Caregiver).GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                string name = propertyInfo.Name;
                if (name != "UpdatedBy" && name != "UpdatedDate" && name!= "lstTag" && name!= "FullName")
                {
                    var newValue = propertyInfo.GetValue(objCaregiver, null);
                    var oldValue = propertyInfo.GetValue(existingCaregiver, null);
                    newValue = (newValue == null) ? string.Empty : newValue;
                    oldValue = (oldValue == null) ? string.Empty : oldValue;
                    lstNewValue.Add(name, newValue.ToString());
                    lstOldValue.Add(name, oldValue.ToString());

                }

            }

            foreach (var item in lstNewValue)
            {
                if (lstOldValue[item.Key] != item.Value)
                {
                    lsFinalValueChange.Add(item.Key, item.Value);
                }
            }
            return lsFinalValueChange;
        }


        /// <summary>
        /// validation check
        /// </summary>
        /// <param name="objCaregiver"></param>
        /// <returns></returns>
        private bool CheckedValidation(Caregiver objCaregiver, ResponseMessage responseMessage)
        {
            return true;
        }




        /// <summary>
        /// Get All Caregiver Giver History By caregiverid
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> GetAllCaregiverGiverHistoryByCaregiverID(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                List<VMCaregiverGiverHistory> lstVMCaregiverGiverHistory = new List<VMCaregiverGiverHistory>();
                int totalCount = 0;
                int totalSkip = 0;
                totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;               

                int caregiverId = JsonConvert.DeserializeObject<int>(requestMessage.RequestObj.ToString());

                //for getting Iqueryable data.
                IQueryable<VMCaregiverGiverHistory> query = _crmDbContext.VMCaregiverGiverHistory
                    .Where(x => x.CaregiverID == caregiverId);

                //for total count
                totalCount = await query.CountAsync();

                //for getting list
                lstVMCaregiverGiverHistory = await query.OrderBy(x => x.CaregiverID).Skip(totalSkip).Take(requestMessage.PageRecordSize).ToListAsync();


                responseMessage.ResponseObj = lstVMCaregiverGiverHistory;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                //Log write
                LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllCaregiverGiverHistoryByCaregiverID");
            }
            catch (Exception ex)
            {
                responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllCaregiverGiverHistoryByCaregiverID");
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }





    }
#pragma warning restore CS8600
#pragma warning restore CS8603

}
