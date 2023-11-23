using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Enums
{
    public class Enums
    {
        public enum Status
        {
            Active = 1,
            Inactive = 2,
            Delete = 9
        }
        public enum ResponseCode
        {
            Success = 1,
            Failed = 2,
            Warning = 3
        }

        public enum DepartmentType
        {
            Patient = 1,
            Aide = 2

        }


        public enum ActionType
        {
            Insert = 1,
            Update = 2,
            View = 3,
            Delete = 4,
            Login = 5,
            Register = 6,
            Logout = 7,
        }
        public enum EmploymentStatus
        {
            Hired = 1,
            Yes = 2,
            No = 3
        }
        public enum Gender
        {
            Male=1,
            Famale=2
        }

        public enum Priority
		{
			LOW = 1,
			MEDIUM,
			HIGH,
			URGENT,
			IMPORTANT
		}

        public enum ScheduleTaskStatus
		{
			Pending = 1,
			Progress,
			Test,
			Completed
		}
    }
}
