using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Constants
{
    public static class MessageConstant
    {
        // Expense  system
        public const string Name_Is_Required = "Name is required.";
        public const string Duplicate_Name = "Duplicate name.";

        public const string RevertSuccessfully = "Revert successfully.";
        public const string SavedSuccessfully = "Saved successfully.";
        public const string SetAsDefaultSuccessfully = "The default has been set successfully.";
        public const string RegisterSuccessfully = "Register successfully.";
        public const string SaveFailed = "Failed to save information.";
        public const string DeleteFailed = "Failed to delete.";
        public const string DeleteSuccess = "Delete successfully.";
        public const string SaveUserssuccess = "User save successfully.";
        public const string SaveUsersFail = "User save successfully.";
        public const string DuplicateUserIDOrEmail = "Duplicate user name or email.";
        public const string DuplicateUserName = "Duplicate user name";
        public const string EmailRequired = "Email must be required";
        public const string UsernameRequired = "Username must be required";
        public const string PasswordRequired = "Password must be required";
        public const string EmailAlreadyExist = "Email already exist";
        public const string PhoneNumberAlreadyExist = "Phone number already exist";
        public const string ConfirmPasswordNotMatch = "Confirm password not matched";
        public const string DepartmentName = "Department name is required.";
        public const string DepartmentNameExist = "This department name is already exist.";
        public const string DepartmentStatusTitle = "Department status title is required.";
        public const string DepartmentTaskID = "Please select department task.";
        public const string TaskDescription = "Task description is required.";
        public const string KnowledgeDetail = "Knowledge detail is required.";
        public const string KnowledgeBase = "Please select knowledge.";
        public const string PermissionName = "Permission name is required.";
        public const string RoleName = "Role name is required.";
        public const string OrganizationName = "Organization name is required.";
        public const string TagTitle = "Tag title is required.";
        public const string TableName = "Table name is required.";
        public const string ColumnName = "Column name is required.";
        public const string ActionName = "Action name is required.";
        public const string DepartmentTaskDescription = "Department task description is required.";
        public const string DepartmentTypeName = "Department type  name is required.";
        public const string Email = "Email name is required.";
        public const string EmployeeName = "EmployeeOld name is required.";
        public const string LoginSuccess = "Login successfully";
        public const string Unauthorizerequest = "Unauthorize request.";
        public const string InternalServerError = "Internal server error.";
        public const string LogOutSuccessfully = "Log out successfully.";
        public const string Invaliddatafound = "Invalid data found.";
        public const string Confirmpasswordnotmatch = "Confirm password not match.";
        public const string PatientNote = "Patient Note is Required.";
        public const string CaregiverNote = "Care giver Note is required.";
        public const string Token = "Token is required.";
        public const string CustomFilterObject = "Filter object is required.";
        public const string TagColor = "Tag color is required.";
        public const string SelectUser = "Please select users.";
        public const string AnyuserNotMapping = "Any user not mapping.";


    }

    public static class CommonConstant
    {
        public static DateTime DeafultDate = Convert.ToDateTime("1900/01/01");
        public static int SessionExpired = 30;
    }

    public static class CommonPath
    {
        public const string loginUrl = "/api/security/login";
        public const string registerUrl = "/api/security/register";
    }
    public class HttpHeaders
    {
        public const string Token = "Authorization";
        public const string AuthenticationSchema = "Bearer";
    }

}
