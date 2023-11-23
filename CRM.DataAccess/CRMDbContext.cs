using CRM.Common.Models;
using CRM.Common.VM;
using Microsoft.EntityFrameworkCore;

namespace CRM.DataAccess
{

#pragma warning disable CS8618


    public class CRMDbContext : DbContext
    {

        public CRMDbContext(DbContextOptions<CRMDbContext> options) : base(options)

        {

        }

        public virtual DbSet<AccessToken> AccessTokens { get; set; }
        public virtual DbSet<Actions> Actions { get; set; }
        public virtual DbSet<Caregiver> Caregiver { get; set; }
        public virtual DbSet<CaregiverHistory> CaregiverHistory { get; set; }
        public virtual DbSet<DefaultLayout> DefaultLayout { get; set; }
        public virtual DbSet<DefaultLayoutDetail> DefaultLayoutDetail { get; set; }
        public virtual DbSet<DefaultTable> DefaultTable { get; set; }
        public virtual DbSet<DefaultTableColumn> DefaultTableColumn { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DepartmentStatus> DepartmentStatus { get; set; }
        public virtual DbSet<DepartmentTask> DepartmentTask { get; set; }
        public virtual DbSet<DepartmentTaskHistory> DepartmentTaskHistory { get; set; }
        public virtual DbSet<KnowledgeBase> KnowledgeBase { get; set; }
        public virtual DbSet<KnowledgeBaseHistory> KnowledgeBaseHistory { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Roles> Role { get; set; }
        public virtual DbSet<RoleActionMapping> RoleActionMapping { get; set; }
        public virtual DbSet<RolePermissionMapping> RolePermissionMapping { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }
        public virtual DbSet<SystemUserOrganizationMapping> SystemUserOrganizationMapping { get; set; }
        public virtual DbSet<SystemUserDepartmentMapping> SystemUserDepartmentMapping { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TagAndDepartmentMapping> TagAndDepartmentMapping { get; set; }
        public virtual DbSet<UserSession> UserSession { get; set; }
        public virtual DbSet<DepartmentTypes> DepartmentType { get; set; }
        public virtual DbSet<VMDepartment> VMDepartment { get; set; }
        public virtual DbSet<VmPatient> VMPatient { get; set; }
        public virtual DbSet<VMPatientLastChange> VMPatientLastChanges { get; set; }
        public virtual DbSet<VMCaregiverLastChange> VMCaregiverLastChange { get; set; }
        public virtual DbSet<PatientHistory> PatientHistory { get; set; }
        public virtual DbSet<EmployeeOld> Employee { get; set; }
        public virtual DbSet<PatientHistoryNew> PatientHistoryNew { get; set; }
        public virtual DbSet<PatientSequence> PatientSequence { get; set; }
        public virtual DbSet<CaregiverHistoryNew> CaregiverHistoryNew { get; set; }
        public virtual DbSet<CaregiverSequence> CaregiverSequence { get; set; }
        public virtual DbSet<CaregiverNotes> CaregiverNotes { get; set; }
        public virtual DbSet<CaregiverAndTagMapping> CaregiverAndTagMapping { get; set; }
        public virtual DbSet<CaregiverAttachment> CaregiverAttachment { get; set; }
        public virtual DbSet<PatientNote> PatientNotes { get; set; }
        public virtual DbSet<VMPatientTag> VMPatientTag { get; set; }
        public virtual DbSet<PatientAndTagMapping> PatientAndTagMapping { get; set; }
        public virtual DbSet<VMCaregiver> VMCaregiver { get; set; }
        public virtual DbSet<VMCaregiverTag> VMCaregiverTag { get; set; }

        public virtual DbSet<PatientAttachment> PatientAttachment { get; set; }
        public virtual DbSet<ScheduleTask> ScheduleTask { get; set; }
        public virtual DbSet<VMScheduleTask> VMScheduleTask { get; set; }
        public virtual DbSet<ScheduleTaskAndTagMapping> ScheduleTaskAndTagMapping { get; set; }
        public virtual DbSet<VMScheduleTaskTag> VMScheduleTaskTag { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<VMOrganization> VMOrganization { get; set; }
        public virtual DbSet<VMChangeUserRole> VMChangeUserRole { get; set; }
        public virtual DbSet<Common.Models.Status> Status { get; set; }
        public virtual DbSet<CaregiverOrganization> CaregiverOrganization { get; set; }
        public virtual DbSet<PatientOrganization> PatientOrganization { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<CaregiverAssignTo> CaregiverAssignTo { get; set; }
        public virtual DbSet<PatientAssignTo> PatientAssignTo { get; set; }
        public virtual DbSet<CaregiverStatus> CaregiverStatus { get; set; }
        public virtual DbSet<PatientStatus> PatientStatus { get; set; }
        public virtual DbSet<CustomFilter> CustomFilter { get; set; }

        public virtual DbSet<VmPatientHistory> VmPatientHistory { get; set; }
        public virtual DbSet<VMCaregiverGiverHistory> VMCaregiverGiverHistory { get; set; }
        public virtual DbSet<VMUserAndDepartmentMapping> VMUserAndDepartmentMapping { get; set; }
        public virtual DbSet<VMDepartmentTag> VMDepartmentTag { get; set; }

        //*** For Expense system
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<ExpenseAttachment> ExpenseAttachment { get; set; }
        public virtual DbSet<ExpenseCategory> ExpenseCategory { get; set; }
        public virtual DbSet<ExpenseType> ExpenseType { get; set; }
        public virtual DbSet<ExpensePermission> ExpensePermission { get; set; }
        public virtual DbSet<ExpenseUserSession> ExpenseUserSession { get; set; }
        public virtual DbSet<ExpenseUser> ExpenseUser { get; set; }

        public virtual DbSet<VMExpenseMonthlySummary> VMExpenseMonthlySummary { get; set; }
        public virtual DbSet<VMExpenseMonthly> VMExpenseMonthly { get; set; }
        //public virtual DbSet<VMExpenseMonthlyAll> VMExpenseMonthlyAll { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //*** For Expense system
            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.ToTable("Expense");
            });
            modelBuilder.Entity<ExpenseAttachment>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.ToTable("ExpenseAttachment");
            });
            modelBuilder.Entity<ExpenseCategory>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.ToTable("ExpenseCategory");
            });
            modelBuilder.Entity<ExpenseType>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.ToTable("ExpenseType");
            });

            modelBuilder.Entity<VMExpenseMonthlySummary>(entity =>
            {
                //entity.HasKey(x => x.ExpenseType);
                entity.HasNoKey();
                entity.ToTable("VMExpenseMonthlySummary");
                // entity.HasNoKey();
                // entity.ToTable("VMExpenseMonthlySummary");
            });
            modelBuilder.Entity<VMExpenseMonthly>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("VMExpenseMonthly");
            });

            //modelBuilder.Entity<VMExpenseMonthlySummary>(entity =>
            //{
            //    entity.HasNoKey();
            //    entity.ToView("GetAllPatient");
            //});


            //** Old */

            modelBuilder.Entity<AccessToken>(entity =>
            {
                entity.HasKey(x => x.AccessTokenID);
                entity.ToTable("AccessTokens");
            });
            modelBuilder.Entity<Actions>(entity =>
            {
                entity.HasKey(x => x.ActionID);
                entity.ToTable("Actions");
            });
            modelBuilder.Entity<Caregiver>(entity =>
            {
                entity.HasKey(x => x.CaregiverID);
                entity.ToTable("Caregiver");
            });
            modelBuilder.Entity<CaregiverHistory>(entity =>
            {
                entity.HasKey(x => x.CaregiverHistoryID);
                entity.ToTable("CaregiverHistory");
            });
            modelBuilder.Entity<DefaultLayout>(entity =>
            {
                entity.HasKey(x => x.DefaultLayoutID);
                entity.ToTable("DefaultLayout");
            });

            modelBuilder.Entity<DefaultLayoutDetail>(entity =>
            {
                entity.HasKey(x => x.DefaultLayoutDetailID);
                entity.ToTable("DefaultLayoutDetail");
            });
            modelBuilder.Entity<DefaultTable>(entity =>
            {
                entity.HasKey(x => x.DefaultTableID);
                entity.ToTable("DefaultTable");
            });

            modelBuilder.Entity<DefaultTableColumn>(entity =>
            {
                entity.HasKey(x => x.DefaultTableColumnID);
                entity.ToTable("DefaultTableColumn");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(x => x.DepartmentID);
                entity.ToTable("Department");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(x => x.OrganizationID);
                entity.ToTable("Organization");
            });


            modelBuilder.Entity<DepartmentStatus>(entity =>
            {
                entity.HasKey(x => x.DepartmentStatusID);
                entity.ToTable("DepartmentStatus");
            });


            modelBuilder.Entity<DepartmentTask>(entity =>
            {
                entity.HasKey(x => x.DepartmentTaskID);
                entity.ToTable("DepartmentTask");
            });


            modelBuilder.Entity<DepartmentTaskHistory>(entity =>
            {
                entity.HasKey(x => x.DepartmentTaskHistoryID);
                entity.ToTable("DepartmentTaskHistory");
            });

            modelBuilder.Entity<KnowledgeBase>(entity =>
             {
                 entity.HasKey(x => x.KnowledgeBaseID);
                 entity.ToTable("KnowledgeBase");
             });

            modelBuilder.Entity<KnowledgeBaseHistory>(entity =>
             {
                 entity.HasKey(x => x.KnowledgeBaseHistoryID);
                 entity.ToTable("KnowledgeBaseHistory");
             });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(x => x.PatientID);
                entity.ToTable("Patient");
            });

            modelBuilder.Entity<PatientAndTagMapping>(entity =>
            {
                entity.HasKey(x => x.PatientAndTagMappingID);
                entity.ToTable("PatientAndTagMapping");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(x => x.PermissionID);
                entity.ToTable("Permission");
            });
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(x => x.RoleID);
                entity.ToTable("Role");
            });
            modelBuilder.Entity<RoleActionMapping>(entity =>
            {
                entity.HasKey(x => x.RoleActionMappingID);
                entity.ToTable("RoleActionMapping");
            });
            modelBuilder.Entity<RolePermissionMapping>(entity =>
            {
                entity.HasKey(x => x.RolePermissionMappingID);
                entity.ToTable("RolePermissionMapping");
            });
            modelBuilder.Entity<SystemUser>(entity =>
            {
                entity.HasKey(x => x.SystemUserID);
                entity.ToTable("SystemUser");
            });
            modelBuilder.Entity<SystemUserOrganizationMapping>(entity =>
            {
                entity.HasKey(x => x.SystemUserOrganizationMappingID);
                entity.ToTable("SystemUserOrganizationMapping");
            });
            modelBuilder.Entity<SystemUserDepartmentMapping>(entity =>
            {
                entity.HasKey(x => x.SystemUserDepartmentMappingID);
                entity.ToTable("SystemUserDepartmentMapping");
            });
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(x => x.TagID);
                entity.ToTable("Tag");
            });

            modelBuilder.Entity<TagAndDepartmentMapping>(entity =>
            {
                entity.HasKey(x => x.TagAndDepartmentMappingID);
                entity.ToTable("TagAndDepartmentMapping");
            });
            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.HasKey(x => x.UserSessionID);
                entity.ToTable("UserSession");
            });

            modelBuilder.Entity<DepartmentTypes>(entity =>
            {
                entity.HasKey(x => x.DepartmentTypeID);
                entity.ToTable("DepartmentType");
            });

            modelBuilder.Entity<VMDepartment>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetAllDepartmment");
            });

            modelBuilder.Entity<VMOrganization>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetAllOrganization");
            });

            modelBuilder.Entity<VMCaregiverLastChange>(entity =>

            {
                entity.HasNoKey();
                entity.ToView("GetCaregiverLastChange");
            });

            modelBuilder.Entity<VmPatient>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetAllPatient");
            });

            modelBuilder.Entity<VMPatientLastChange>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetPatientLastChange");
            });

            modelBuilder.Entity<VMPatientTag>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetPatientTag");
            });

            modelBuilder.Entity<PatientHistory>(entity =>
            {
                entity.HasKey(x => x.PatientHistoryID);
                entity.ToTable("PatientHistory");
            });
            modelBuilder.Entity<EmployeeOld>(entity =>
            {
                entity.HasKey(x => x.EmployeeID);
                entity.ToTable("EmployeeOld");
            });

            modelBuilder.Entity<PatientHistoryNew>(entity =>
            {
                entity.HasKey(x => x.HistoryID);
                entity.ToTable("PatientHistoryNew");
            });

            modelBuilder.Entity<PatientSequence>(entity =>
            {
                entity.HasKey(x => x.PatientSequenceID);
                entity.ToTable("PatientSequence");
            });

            modelBuilder.Entity<CaregiverHistoryNew>(entity =>
            {
                entity.HasKey(x => x.HistoryID);
                entity.ToTable("CaregiverHistoryNew");
            });
            modelBuilder.Entity<CaregiverSequence>(entity =>
            {
                entity.HasKey(x => x.CaregiverSequenceID);
                entity.ToTable("CaregiverSequence");
            });
            modelBuilder.Entity<CaregiverNotes>(entity =>
            {
                entity.HasKey(x => x.CaregiverNoteID);
                entity.ToTable("CaregiverNotes");
            });

            modelBuilder.Entity<CaregiverAndTagMapping>(entity =>
            {
                entity.HasKey(x => x.CaregiverAndTagMappingID);
                entity.ToTable("CaregiverAndTagMapping");
            });

            modelBuilder.Entity<CaregiverAttachment>(entity =>
            {
                entity.HasKey(x => x.CaregiverAttachmentID);
                entity.ToTable("CaregiverAttachment");
            });
            modelBuilder.Entity<VMCaregiver>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetAllCaregiver");
            });
            modelBuilder.Entity<VMCaregiverTag>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetCaregiverTag");
            });

            modelBuilder.Entity<ScheduleTask>(entity =>
            {
                entity.HasKey(k => k.ScheduleTaskID);
                entity.ToTable("ScheduleTask");
            });

            modelBuilder.Entity<VMScheduleTask>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetAllScheduleTask");
            });

            modelBuilder.Entity<ScheduleTaskAndTagMapping>(entity =>
            {
                entity.HasKey(k => k.ScheduleTaskAndTagID);
                entity.ToTable("ScheduleTaskAndTagMapping");
            });

            modelBuilder.Entity<VMScheduleTaskTag>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetScheduleTaskTag");
            });
            modelBuilder.Entity<VMChangeUserRole>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("ChangeRole");
            });

            modelBuilder.Entity<Common.Models.Status>(entity =>
            {
                entity.HasKey(k => k.StatusID);
                entity.ToTable("Status");
            });

            modelBuilder.Entity<CaregiverOrganization>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetHeadOrganizationWiseOldAndNewCaregiver");
            });

            modelBuilder.Entity<PatientOrganization>(entity =>
             {
                 entity.HasNoKey();
                 entity.ToView("GetHeadOrganizationWiseOldAndNewPatient");
             });

            modelBuilder.Entity<CaregiverAssignTo>(entity =>
             {
                 entity.HasNoKey();
                 entity.ToView("GetHeadCaregiverAssine");
             });

            modelBuilder.Entity<PatientAssignTo>(entity =>
             {
                 entity.HasNoKey();
                 entity.ToView("GetHeadPatientAssine");
             });

            modelBuilder.Entity<CaregiverStatus>(entity =>
             {
                 entity.HasNoKey();
                 entity.ToView("GetHeadCaregiverStatus");
             });
            modelBuilder.Entity<PatientStatus>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetHeadpatientStatus");
            });

            modelBuilder.Entity<CustomFilter>(entity =>
            {
                entity.HasKey(x => x.CustomFilterID);
                entity.ToTable("CustomFilter");
            });

            modelBuilder.Entity<VmPatientHistory>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetPatientHistory");
            });
            modelBuilder.Entity<VMCaregiverGiverHistory>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetCaregiverGiverHistory");
            });

            modelBuilder.Entity<VMUserAndDepartmentMapping>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetSystemUsersByDepartmentID");
            });

            modelBuilder.Entity<VMDepartmentTag>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("GetAllDepartmentTags");
            });
        }
    }

#pragma warning restore CS8618
}