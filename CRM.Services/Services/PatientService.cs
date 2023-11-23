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
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static CRM.Common.Enums.Enums;

namespace CRM.Services
{
#pragma warning disable CS8600
	public class PatientService : IPatientService
	{
		private readonly CRMDbContext _crmDbContext;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly BuildDynamicFilter _buildDynamicFilter;
		public PatientService(CRMDbContext ctx, IServiceScopeFactory serviceScopeFactor, BuildDynamicFilter buildDynamicFilter)
		{
			_crmDbContext = ctx;
			_serviceScopeFactory = serviceScopeFactor;
			_buildDynamicFilter = buildDynamicFilter;
		}

		/// <summary>
		/// Get all Patient
		/// </summary>
		/// <param name="requestMessage"></param>
		/// <returns></returns>
		public async Task<ResponseMessage> GetAllPatient(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			try
			{
				List<Patient> lstPatient = new List<Patient>();
				int totalSkip = 0;
				totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;
				VMGetAllPatient objGetAllPatient = JsonConvert.DeserializeObject<VMGetAllPatient>(requestMessage.RequestObj?.ToString() ?? "");
				var patientQuery = _crmDbContext.Patient
				  .Where(x => x.DepartmentID == objGetAllPatient.DepartmentId &&
				  (string.IsNullOrEmpty(objGetAllPatient.SearchText) || x.FirstName.Contains(objGetAllPatient.SearchText)))
				  .OrderBy(x => x.PatientID)
				  .Skip(totalSkip)
				  .Take(requestMessage.PageRecordSize);
				lstPatient = await patientQuery.ToListAsync();
				responseMessage.ResponseObj = lstPatient;
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

				//Log write
				LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllPatient");
			}
			catch (Exception ex)
			{
				//Process excetion, Development mode show real exception and production mode will show custom exception.
				responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllPatient");
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
			}

			return responseMessage;
		}

		public async Task<ResponseMessage> GetAllPatientInitData(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			try
			{
				VMPatientInit objVMCaregiver = new VMPatientInit();

				objVMCaregiver.lstOrganization = await GetOrganizationWithValue();
				objVMCaregiver.lstAssignTo = await GetAllAssignewithValue(requestMessage.UserID);
				objVMCaregiver.lstSource = await GetAllSource();
				objVMCaregiver.lstTags = await GetAllTagsValue();
				objVMCaregiver.lstStatus = await GetAllStatusWithValue();

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

		private async Task<List<PatientOrganization>> GetOrganizationWithValue()
		{

			List<PatientOrganization> lstPatientOrganization = new List<PatientOrganization>();
			lstPatientOrganization = await _crmDbContext.PatientOrganization.ToListAsync();

			return lstPatientOrganization;
		}

		//TODO: Zahid
		private async Task<List<PatientTags>> GetAllTagsValue()
		{
			var scheduleTaskTagQuery = _crmDbContext.VMPatientTag
				.GroupBy(g => g.TagID)
				.Select(s => new PatientTags()
				{
					TagsName = s.FirstOrDefault().TagTitle,
					ColorCode = s.FirstOrDefault().Color,
					TotalTagsValue = s.Count()
				});

			List<PatientTags> lstTag = await scheduleTaskTagQuery.ToListAsync();

			return lstTag;
		}

		//TODO: Zahid
		private async Task<List<PatientStatus>> GetAllStatusWithValue()
		{
			List<PatientStatus> lstStatus = new List<PatientStatus>();
			lstStatus = await _crmDbContext.PatientStatus.ToListAsync();
			return lstStatus;
		}

		//TODO: Zahid
		private async Task<List<PatientAssignTo>> GetAllAssignewithValue(int userid)
		{

			var lstScheduleTaskAssignee = await _crmDbContext.ScheduleTask.Where(x => x.RelatedTo == 1 && x.AssigneeID > 0)
				.OrderBy(o => o.ScheduleTaskID).GroupBy(g => g.AssigneeID).Select(s => new PatientAssignTo()
				{
					AssignID = s.Key,
					TotalAssignValue = s.Count()
				}).ToListAsync();

			List<PatientAssignTo> lstPatientAssignTo = new List<PatientAssignTo>();

			lstPatientAssignTo = await _crmDbContext.PatientAssignTo.ToListAsync();

			if (lstPatientAssignTo.Count > 0)
			{
				foreach (PatientAssignTo item in lstPatientAssignTo)
				{
					if (item.AssignID == userid)
					{
						item.AssignName = "Me";
					}
				}
			}
			return lstPatientAssignTo;
		}

		//TODO: Zahid
		private async Task<List<PatientSource>> GetAllSource()
		{
			List<PatientSource> lstSource = await _crmDbContext.Patient.GroupBy(g => g.Source).Select(s => new PatientSource()
			{
				SourceName = s.Key,
				TotalSourceValue = s.Count()
			}).ToListAsync();

			return lstSource;
		}

		/// <summary>
		/// Get all Patient
		/// </summary>
		/// <param name="requestMessage"></param>
		/// <returns></returns>
		public async Task<ResponseMessage> GetAllPatientList(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			try
			{
				VMPatient objVmPatients = new VMPatient();
				List<VmPatient> lstPatient = new List<VmPatient>();
				int totalSkip = 0;
				totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

				lstPatient = await _crmDbContext.VMPatient
				  .OrderBy(x => x.PatientID)
				  .Skip(totalSkip)
				  .Take(requestMessage.PageRecordSize)
				  .ToListAsync();

				List<int> lstPatientId = lstPatient.Select(s => s.PatientID).Distinct().ToList();
				var lstPatientQuery = _crmDbContext.VMPatientTag.Where(x => lstPatientId.Contains((int?)x.PatientID ?? 0));
				List<VMPatientTag> lstPatientTag = await lstPatientQuery.ToListAsync();
				if (lstPatient.Count > 0)
				{
					lstPatient.ForEach(patient =>
					{
						patient.lstTag = lstPatientTag.Where(x => x.PatientID == patient.PatientID).ToList();
					});
					int[] lstPatientIds = lstPatient.Select(x => x.PatientID).Distinct().ToArray();
					objVmPatients.lstPatientHistory = await _crmDbContext.VMPatientLastChanges
					  .Where(p => lstPatientIds.Contains((int?)p.PatientID ?? 0)).ToListAsync();
				}
				objVmPatients.lstPaitent = lstPatient;
				responseMessage.ResponseObj = objVmPatients;
				responseMessage.TotalCount = await _crmDbContext.VMPatient.CountAsync();
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

				//Log write
				LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllPatientWithChange");
			}
			catch (Exception ex)
			{
				//Process excetion, Development mode show real exception and production mode will show custom exception.
				responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllPatientWithChange");
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
			}

			return responseMessage;
		}

		public async Task<ResponseMessage> GetAllPatientChangeHistoryList(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			try
			{
				List<VmPatientHistory> lstPatientHistory = new List<VmPatientHistory>();
				int totalSkip = 0;
				int departmentId = 0;
				DateTime formDate = DateTime.Now.AddDays(-7);
				totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

				QueryObject objQueryObject = JsonConvert.DeserializeObject<QueryObject>(requestMessage.RequestObj.ToString());

				_ = int.TryParse(objQueryObject.BasicSearch.ToString(), out departmentId);

				if (objQueryObject?.LstColumnName != null && objQueryObject?.LstColumnName[0] == "FullName")
				{
					objQueryObject?.LstColumnName.Add("FirstName");
					objQueryObject?.LstColumnName.Add("LastName");
				}


				IQueryable<VmPatientHistory> query = _crmDbContext.VmPatientHistory
					.Where(x => x.DepartmentID == departmentId &&
					(objQueryObject.FromDate == null ? formDate.Date < x.ExecutionDate : objQueryObject.FromDate < x.ExecutionDate && x.ExecutionDate <= objQueryObject.ToDate)
					&& (objQueryObject.LstColumnName == null || objQueryObject.LstColumnName.Contains(x.ColumnName)));

				lstPatientHistory = await query
				  .OrderBy(x => x.PatientID)
				  .Skip(totalSkip)
				  .Take(requestMessage.PageRecordSize)
				  .ToListAsync();

				responseMessage.ResponseObj = lstPatientHistory;
				responseMessage.TotalCount = await query.CountAsync();
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

				//Log write
				LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllPatientChangeHistoryList");
			}
			catch (Exception ex)
			{
				//Process excetion, Development mode show real exception and production mode will show custom exception.
				responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllPatientChangeHistoryList");
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
			}

			return responseMessage;
		}

		public async Task<ResponseMessage> GetPatientChangeHistoryList(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			try
			{
				List<VmPatientHistory> lstPatientHistory = new List<VmPatientHistory>();
				int totalSkip = 0;
				int patientID = 0;
				totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

				_ = int.TryParse(requestMessage.RequestObj.ToString(), out patientID);

				IQueryable<VmPatientHistory> query = _crmDbContext.VmPatientHistory
					.Where(x => x.PatientID == patientID);

				lstPatientHistory = await query
				  .OrderBy(x => x.PatientID)
				  .Skip(totalSkip)
				  .Take(requestMessage.PageRecordSize)
				  .ToListAsync();

				responseMessage.ResponseObj = lstPatientHistory;
				responseMessage.TotalCount = await query.CountAsync();
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

				//Log write
				LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetPatientChangeHistoryList");
			}
			catch (Exception ex)
			{
				//Process excetion, Development mode show real exception and production mode will show custom exception.
				responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetPatientChangeHistoryList");
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
			}

			return responseMessage;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="requestMessage"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<ResponseMessage> GetPatientById(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			try
			{
				Patient objPatient = new Patient();
				int PatientID = JsonConvert.DeserializeObject<int>(requestMessage?.RequestObj.ToString());

				objPatient = await _crmDbContext.Patient.FirstOrDefaultAsync(x => x.PatientID == PatientID && x.Status == (int)Enums.Status.Active);
				responseMessage.ResponseObj = objPatient;
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

				//Log write
				LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetPatientById");
			}
			catch (Exception ex)
			{
				//Process excetion, Development mode show real exception and production mode will show custom exception.
				responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetPatientById");
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
			}

			return responseMessage;
		}

		/// <summary>
		/// Save and update Patient
		/// </summary>
		/// <param name="requestMessage"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<ResponseMessage> SavePatient(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			int actionType = (int)Enums.ActionType.Insert;
			try
			{
				Patient objPatient = JsonConvert.DeserializeObject<Patient>(requestMessage?.RequestObj.ToString());

				if (objPatient != null)
				{
					if (CheckedValidation(objPatient, responseMessage))
					{
						if (objPatient.PatientID > 0)
						{
							Patient existingPatient = await _crmDbContext.Patient.AsNoTracking().FirstOrDefaultAsync(x => x.PatientID == objPatient.PatientID && x.Status == (int)Enums.Status.Active);
							if (existingPatient != null)
							{
								actionType = (int)Enums.ActionType.Update;
								objPatient.CreatedDate = existingPatient.CreatedDate;
								objPatient.CreatedBy = existingPatient.CreatedBy;
								objPatient.UpdatedDate = DateTime.Now;
								objPatient.UpdatedBy = requestMessage.UserID;
								_crmDbContext.Patient.Update(objPatient);

								List<PatientAndTagMapping> lstPatientTag = await _crmDbContext.PatientAndTagMapping.Where(x => x.PatientID == objPatient.PatientID).ToListAsync();
								if (lstPatientTag != null)
								{
									List<int?> existingTagIDs = lstPatientTag.Select(s => s.TagID).Distinct().ToList();
									var lstNewPatientTag = objPatient.lstTag.Where(x => !existingTagIDs.Contains(x.TagID))
										.Select(s => new PatientAndTagMapping()
										{
											PatientID = objPatient.PatientID,
											TagID = s.TagID,
											Status = (int)Enums.Status.Active
										})
										.ToList();
									await _crmDbContext.PatientAndTagMapping.AddRangeAsync(lstNewPatientTag);

									//delete exting record but not came in client side 
									var newTagsIDs = objPatient?.lstTag?.Select(s => s.TagID).Distinct().ToList();
									var lstDeleteTag = lstPatientTag?.Where(x => !newTagsIDs.Contains(x.TagID))
										.Select(s => new PatientAndTagMapping()
										{
											PatientAndTagMappingID = s.PatientAndTagMappingID,
											PatientID = objPatient.PatientID,
											TagID = s.TagID,
											Status = (int)Enums.Status.Delete
										})
										.ToList();
									_crmDbContext.PatientAndTagMapping.UpdateRange(lstDeleteTag);
								}
								else
								{
									var lstNewPatientTag = objPatient.lstTag.Select(s => new PatientAndTagMapping()
									{
										PatientID = objPatient.PatientID,
										TagID = s.TagID,
										Status = (int)Enums.Status.Active
									}).ToList();
									await _crmDbContext.PatientAndTagMapping.AddRangeAsync(lstNewPatientTag);
								}
								await _crmDbContext.SaveChangesAsync();
								InsertPatientHistory(objPatient, existingPatient, requestMessage.UserID);//for update save change method used in InsertPatientHistory this method. because their has used transaction
							}
						}
						else
						{
							objPatient.Status = (int)Enums.Status.Active;
							objPatient.CreatedDate = DateTime.Now;
							objPatient.CreatedBy = requestMessage.UserID;
							await _crmDbContext.Patient.AddAsync(objPatient);
							await _crmDbContext.SaveChangesAsync();
							if (objPatient.lstTag != null)
							{
								var lstNewPatientTag = objPatient.lstTag.Select(s => new PatientAndTagMapping()
								{
									PatientID = objPatient.PatientID,
									TagID = s.TagID,
									Status = (int)Enums.Status.Active
								}).ToList();
								await _crmDbContext.PatientAndTagMapping.AddRangeAsync(lstNewPatientTag);
							}
							await _crmDbContext.SaveChangesAsync();
						}

						responseMessage.ResponseObj = objPatient;
						responseMessage.Message = MessageConstant.SavedSuccessfully;
						responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

						//Log write
						LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "SavePatient");
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

		public async Task<ResponseMessage> GetPatientsForDropdown(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			try
			{
				string searchText = requestMessage.RequestObj?.ToString();
				List<VmPatient> lstPatinet = await _crmDbContext.VMPatient
				  .Where(x => x.Status == (int)Enums.Status.Active &&
				  ( string.IsNullOrEmpty(searchText) == true || ( x.FirstName.Contains(searchText) || x.LastName.Contains(searchText) || x.DepartmentName.Contains(searchText) ) ))
				  .Select(s => new VmPatient()
				  {
					  PatientID = s.PatientID,
					  FirstName = s.FirstName,
					  LastName = s.LastName,
					  Gender = s.Gender,
					  DepartmentID = s.DepartmentID,
					  DepartmentName = s.DepartmentName
				  })
				  .Take(10)
				  .ToListAsync();

				responseMessage.ResponseObj = lstPatinet;
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
			}
			catch (Exception ex)
			{
				//Process excetion, Development mode show real exception and production mode will show custom exception.
				responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "SavePatient");
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
			}
			return responseMessage;
		}

		//TODO:zahid
		/// <summary>
		/// Get all column value changes old and new 
		/// </summary>
		/// <param name="patient"></param>
		/// <param name="existingPatient"></param>
		/// <param name="lstOldValue"></param>
		/// <returns></returns>
		private Dictionary<string, string> GetAllChangeValue(Patient patient, Patient existingPatient, out Dictionary<string, string> lstOldValue)
		{
			Dictionary<string, string> lstNewValue = new Dictionary<string, string>();
			lstOldValue = new Dictionary<string, string>();
			Dictionary<string, string> lsFinalValueChange = new Dictionary<string, string>();

			PropertyInfo[] propertyInfos;
			propertyInfos = typeof(Patient).GetProperties();

			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				string name = propertyInfo.Name;
				var newValue = propertyInfo.GetValue(patient, null);
				var oldValue = propertyInfo.GetValue(existingPatient, null);
				newValue = (newValue == null) ? string.Empty : newValue;
				oldValue = (oldValue == null) ? string.Empty : oldValue;
				lstNewValue.Add(name, newValue.ToString());
				lstOldValue.Add(name, oldValue.ToString());
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
		/// Save patient history.
		/// </summary>
		/// <param name="patient"></param>
		/// <param name="existingPatient"></param>
		/// <param name="userId"></param>
		private void InsertPatientHistory(Patient patient, Patient existingPatient, int userId)
		{
			PatientSequence objPatientSequence = new PatientSequence();
			List<PatientHistoryNew> lstPatientHistoryNew = new List<PatientHistoryNew>();
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var db = scope.ServiceProvider.GetService<CRMDbContext>();
				using var transaction = db.Database.BeginTransaction();
				try
				{

					Dictionary<string, string> lsFinalValueChange = new Dictionary<string, string>();
					Dictionary<string, string> lstOldValue = new Dictionary<string, string>();
					lsFinalValueChange = GetAllChangeValue(patient, existingPatient, out lstOldValue);

					objPatientSequence = db.PatientSequence.OrderBy(x => x.PatientSequenceID).LastOrDefault();
					int seqNo = (objPatientSequence != null && objPatientSequence.PatientSequenceNo > 0) ? objPatientSequence.PatientSequenceNo + 1 : 1;

					foreach (var item in lsFinalValueChange)
					{
						PatientHistoryNew objPatientHistoryNew = new PatientHistoryNew();
						objPatientHistoryNew.PatientID = patient.PatientID;
						objPatientHistoryNew.SequenceNo = seqNo;
						objPatientHistoryNew.ColumnName = item.Key;
						objPatientHistoryNew.OldValue = lstOldValue[item.Key];
						objPatientHistoryNew.NewValue = item.Value;
						objPatientHistoryNew.CreatedDate = DateTime.Now;
						objPatientHistoryNew.CreatedBy = userId;

						//for save in exception log table if any exception happen
						lstPatientHistoryNew.Add(objPatientHistoryNew);

						db.PatientHistoryNew.Add(objPatientHistoryNew);

					}
					if (lsFinalValueChange.Count > 0)
					{
						objPatientSequence = new PatientSequence();
						objPatientSequence.PatientSequenceNo = seqNo;
						db.PatientSequence.Add(objPatientSequence);
					}
					db.SaveChanges();

					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.Insert, userId, JsonConvert.SerializeObject(lstPatientHistoryNew), "InsertPatientHistory");
				}
			}

		}

		/// <summary>
		/// validation check
		/// </summary>
		/// <param name="objPatient"></param>
		/// <returns></returns>
		private bool CheckedValidation(Patient objPatient, ResponseMessage responseMessage)
		{
			return true;
		}

		public async Task<ResponseMessage> GetAllVMPatientByDepartmentID(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			try
			{
				int totalCount = 0;
				VMPatient objVmPatients = new VMPatient();
				List<VmPatient> lstPatient = new List<VmPatient>();
				List<VMPatientTag> lstPatientTag = new List<VMPatientTag>();
				string queryString = "Select * from [dbo].[GetAllPatient] where";

				int departmentID = 0;
				int totalSkip = 0;
				totalSkip = (requestMessage.PageNumber > 0) ? requestMessage.PageNumber * requestMessage.PageRecordSize : 0;

				QueryObject objQueryObject = JsonConvert.DeserializeObject<QueryObject>(requestMessage.RequestObj.ToString());
				_ = int.TryParse(objQueryObject.BasicSearch.ToString(), out departmentID);

				string whereQuery = " [DepartmentID] = " + departmentID;

				//for global search.
				if (objQueryObject.SearchText != null && objQueryObject.FilterModel?.Count < 1)
				{
					whereQuery += " AND (LTRIM(RTRIM([FirstName])) + ' ' + LTRIM(RTRIM([LastName]))) like '%" + objQueryObject.SearchText + "%'"
								  + " OR HomePhone like '%" + objQueryObject.SearchText + "%'" + " OR Email like '%" + objQueryObject.SearchText + "%'"
								  + " OR City like '%" + objQueryObject.SearchText + "%'" + " OR Zip like '%" + objQueryObject.SearchText + "%'";
				}

				//for generate filter.
				if (objQueryObject?.FilterModel != null && objQueryObject.FilterModel.Count > 0)
				{
					whereQuery += _buildDynamicFilter.GetFilterCluse(objQueryObject.FilterModel);
				}

				//need always  call before this GenerateSortQuery for total count.
				totalCount = await _crmDbContext.VMPatient.FromSqlRaw("SELECT [PatientID] FROM [dbo].[GetAllPatient] where" + whereQuery).CountAsync();

				//for sort column
				if (objQueryObject?.SortModel?.Count > 0)
				{
					whereQuery += _buildDynamicFilter.GenerateSortQuery(objQueryObject.SortModel);
				}

				queryString += whereQuery;

				//check condition
				if (objQueryObject?.SortModel?.Count > 0)
				{
					queryString += $"  OFFSET {totalSkip} ROWS FETCH NEXT {requestMessage.PageRecordSize} ROWS ONLY";
				}
				else
				{
					queryString += $" ORDER BY [PatientID] DESC OFFSET {totalSkip} ROWS FETCH NEXT {requestMessage.PageRecordSize} ROWS ONLY";
				}

				lstPatient = await _crmDbContext.VMPatient.FromSqlRaw(queryString).ToListAsync();

				List<int> lstPatientId = lstPatient.Select(s => s.PatientID).Distinct().ToList();
				var lstPatientQuery = _crmDbContext.VMPatientTag.Where(x => lstPatientId.Contains((int?)x.PatientID ?? 0));
				lstPatientTag = await lstPatientQuery.ToListAsync();

				if (lstPatient.Count > 0)
				{
					lstPatient.ForEach(patient =>
					{
						patient.lstTag = lstPatientTag.Where(x => x.PatientID == patient.PatientID).ToList();
					});

					objVmPatients.lstPatientHistory = await _crmDbContext.VMPatientLastChanges.Where(p => lstPatientId.Contains((int?)p.PatientID ?? 0)).ToListAsync();
				}

				objVmPatients.lstPaitent = lstPatient;
				responseMessage.ResponseObj = objVmPatients;
				responseMessage.TotalCount = totalCount;
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

				//Log write
				LogHelper.WriteLog(requestMessage?.RequestObj, (int)Enums.ActionType.View, requestMessage.UserID, "GetAllPatientWithChange");
			}
			catch (Exception ex)
			{
				//Process excetion, Development mode show real exception and production mode will show custom exception.
				responseMessage.Message = ExceptionHelper.ProcessException(ex, (int)Enums.ActionType.View, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "GetAllPatientWithChange");
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
		public async Task<ResponseMessage> RevertPatient(RequestMessage requestMessage)
		{
			ResponseMessage responseMessage = new ResponseMessage();
			int actionType = (int)Enums.ActionType.Insert;
			try
			{
				VmPatientHistory objVmPatientHistory = JsonConvert.DeserializeObject<VmPatientHistory>(requestMessage?.RequestObj.ToString());

				Patient objExistingPatient = new Patient();
				PatientSequence objPatientSequence = new PatientSequence();

				if (objVmPatientHistory != null)
				{
					if (CheckedPatientHistoryValidation(objVmPatientHistory, responseMessage))
					{

						if (objVmPatientHistory.PatientID > 0)
						{
							objExistingPatient = await _crmDbContext.Patient.AsNoTracking().FirstOrDefaultAsync(x => x.PatientID == objVmPatientHistory.PatientID && x.Status == (int)Enums.Status.Active);
							if (objExistingPatient != null)
							{
								objPatientSequence = _crmDbContext.PatientSequence.OrderBy(x => x.PatientSequenceID).AsNoTracking().LastOrDefault();
								int seqNo = (objPatientSequence != null && objPatientSequence.PatientSequenceNo > 0) ? objPatientSequence.PatientSequenceNo + 1 : 1;

								//mapping data with caregiver.
								MappedColumnData(objExistingPatient, objVmPatientHistory);

								actionType = (int)Enums.ActionType.Update;
								objExistingPatient.UpdatedDate = DateTime.Now;
								objExistingPatient.UpdatedBy = requestMessage.UserID;

								_crmDbContext.Patient.Update(objExistingPatient);

								//mapp data..
								PatientHistoryNew objPatientHistoryNew = new PatientHistoryNew();
								objPatientHistoryNew.PatientID = objVmPatientHistory.PatientID;
								objPatientHistoryNew.SequenceNo = seqNo;
								objPatientHistoryNew.ColumnName = objVmPatientHistory.ColumnName;
								objPatientHistoryNew.OldValue = objVmPatientHistory.NewValue;
								objPatientHistoryNew.NewValue = objVmPatientHistory.OldValue;
								objPatientHistoryNew.CreatedDate = DateTime.Now;
								objPatientHistoryNew.CreatedBy = requestMessage.UserID;

								await _crmDbContext.PatientHistoryNew.AddAsync(objPatientHistoryNew);
								await _crmDbContext.SaveChangesAsync();

							}
						}


						responseMessage.ResponseObj = objExistingPatient;
						responseMessage.Message = MessageConstant.RevertSuccessfully;
						responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;

						//Log write
						LogHelper.WriteLog(requestMessage.RequestObj, actionType, requestMessage.UserID, "RevertPatient");
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
				responseMessage.Message = ExceptionHelper.ProcessException(ex, actionType, requestMessage.UserID, JsonConvert.SerializeObject(requestMessage.RequestObj), "RevertPatient");
				responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;

			}

			return responseMessage;
		}
		/// <summary>
		/// method for mapping data.
		/// </summary>
		/// <param name="objExistingPatient"></param>
		/// <param name="objVmPatientHistory"></param>
		private void MappedColumnData(Patient objExistingPatient, VmPatientHistory objVmPatientHistory)
		{

			PropertyInfo[] propertyInfos;
			propertyInfos = typeof(Patient).GetProperties();
			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				string colname = propertyInfo.Name;
				if (colname == objVmPatientHistory.ColumnName)
				{
					propertyInfo.SetValue(objExistingPatient, objVmPatientHistory.OldValue);
					break;

				}
			}
		}

		private bool CheckedPatientHistoryValidation(VmPatientHistory objVMCaregiverGiverHistory, ResponseMessage responseMessage)
		{
			return true;
		}
	}
#pragma warning restore CS8600

}
