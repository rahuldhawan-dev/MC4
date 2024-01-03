using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Operations.Models.ViewModels
{
    public class AbsenceNotificationViewModel : ViewModel<AbsenceNotification>
    {
        #region Properties

        [DoesNotAutoMap("Only used for cascading.")]
        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("e.OperatingCenter", "oc", "Id")]
        public int? OperatingCenter { get; set; }

        [Required]
        [DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above.")]
        [EntityMap("Employee"), EntityMustExist(typeof(Employee))]        
        public int? Employee { get; set; }   
        
        public DateTime? LastDayOfWork { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalHoursOfAbsence { get; set; }
        [DropDown]
        [EntityMap("EmployeeAbsenceClaim"), EntityMustExist(typeof(EmployeeAbsenceClaim))]
        public int? EmployeeAbsenceClaim { get; set; }
        public string SupervisorNotes { get; set; }
        [DropDown("Operations", "FamilyMedicalLeaveActCase", "GetByEmployeeId", DependsOn = "Employee", PromptText = "Please select an employee")]
        [EntityMap("FamilyMedicalLeaveActCase"), EntityMustExist(typeof(FamilyMedicalLeaveActCase))]
        public int? FamilyMedicalLeaveActCase { get; set; }
        public bool HumanResourcesReviewed { get; set; }
        public string HumanResourcesNotes { get; set; }
        public DateTime? PackageDateSent { get; set; }
        public DateTime? PackageDateDue { get; set; }
        public bool ReturnToWorkNote { get; set; }
        public DateTime? ProgressiveDisciplineAdministered { get; set; }
        [DropDown, Required]
        [EntityMap("EmployeeFMLANotification"), EntityMustExist(typeof(EmployeeFMLANotification))]
        public int? EmployeeFMLANotification { get; set; }

        [DropDown]
        [EntityMap("ProgressiveDiscipline"), EntityMustExist(typeof(ProgressiveDiscipline))]
        public int? ProgressiveDiscipline { get; set; }

        [DropDown, RequiredWhen("HumanResourcesReviewed", true)]
        [EntityMap("AbsenceStatus"), EntityMustExist(typeof(AbsenceStatus))]
        public int? AbsenceStatus { get; set; }

        #endregion

        #region Constructors

        public AbsenceNotificationViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateAbsenceNotification : AbsenceNotificationViewModel
    {
        #region Constructors

        public CreateAbsenceNotification(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override AbsenceNotification MapToEntity(AbsenceNotification entity)
        {
            base.MapToEntity(entity);
            var curUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            entity.SubmittedBy = curUser;
            return entity;
        }

        #endregion
    }

    public class EditAbsenceNotification : AbsenceNotificationViewModel
    {
        #region Exposed Methods

        public override void Map(AbsenceNotification entity)
        {
            OperatingCenter = entity.Employee?.OperatingCenter?.Id;
            base.Map(entity);
        }

        #endregion

        #region Constructors

		public EditAbsenceNotification(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchAbsenceNotification : SearchSet<AbsenceNotification>
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("e.OperatingCenter", "oc", "Id")]
        public int? OperatingCenter { get; set; }      
        [DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above."), EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }
        [SearchAlias("Employee", "e", "LastName")]
        public string LastName { get; set; }
        [SearchAlias("e.ReportsTo", "LastName")]       
        public string ReportsTo { get; set; }
        public DateRange LastDayOfWork { get; set; }
        [View("Absence Start Date")]
        public DateRange StartDate { get; set; }
        public DateRange EndDate { get; set; }
        public DateRange CreatedAt { get; set; }
        [View("FMLA Case ID")]
        public int? FamilyMedicalLeaveActCase { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(AbsenceStatus))]
        public int? AbsenceStatus { get; set; }
        [View("Local Management Reviewed")]
        public bool? HumanResourcesReviewed { get; set; }

        [EntityMap, EntityMustExist(typeof(ProgressiveDiscipline))]
        [DropDown, View("Progressive Discipline Step")]
        public int? ProgressiveDiscipline { get; set; }
        public DateRange ProgressiveDisciplineAdministered { get; set; }

        #endregion
    }

    public class SearchOccurrence : SearchSet<OccurrenceReportItem>
    {
        [DropDown]
        [SearchAlias("employee.OperatingCenter", "operatingCenter", "Id")]
        public int? OperatingCenter { get; set; }
        [DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public int? Employee { get; set; }
    }
}
