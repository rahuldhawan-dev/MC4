using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HumanResources.Models.ViewModels
{

    public abstract class EmployeeAccountabilityActionViewModel : ViewModel<EmployeeAccountabilityAction>
    {
        #region Fields

        private Employee _employee;

        #endregion

        #region Properties

        #region Original

        [Required]
        [EntityMap, EntityMustExist(typeof(Employee))]
        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an Operating Center above")]
        public virtual int? Employee { get; set; }
        [Required, DropDown, EntityMustExist(typeof(Employee)), EntityMap]
        public virtual int? DisciplineAdministeredBy { get; set; }
        [View("Employee")]
        [Required, DoesNotAutoMap("Display only")]
        public virtual Employee EmployeeDisplay
        {
            get
            {
                if (_employee == null)
                {
                    _employee = _container.GetInstance<IRepository<Employee>>()
                                          .Find(Employee.GetValueOrDefault());
                }

                return _employee;
            }
        }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(AccountabilityActionTakenType))]
        public virtual int? AccountabilityActionTakenType { get; set; }
        [Required, Multiline]
        [StringLength(EmployeeAccountabilityAction.StringLengths.ACCOUNTABILITY_ACTION_TAKEN_DESCRIPTION)]
        public virtual string AccountabilityActionTakenDescription { get; set; }
        [Required]
        public virtual DateTime? DateAdministered { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual int? NumberOfWorkDays { get; set; }

        [EntityMap, EntityMustExist(typeof(Incident))]
        [DropDown("", "Incident", "ByEmployeeId", PromptText = "Please select an employee above.",
            DependsOn = "Employee")]
        public virtual int? Incident { get; set; }

        [EntityMap, EntityMustExist(typeof(Grievance))]
        [DropDown("", "Grievance", "ByEmployeeId", PromptText = "Please select an employee above.",
            DependsOn = "Employee")]
        public virtual int? Grievance { get; set; }

        #endregion

        #region Modified

        public virtual bool HasModifiedDiscipline { get; set; }
        [RequiredWhen("HasModifiedDiscipline", ComparisonType.EqualTo, true)]
        [DropDown, EntityMustExist(typeof(Employee)), EntityMap]
        public virtual int? ModifiedDisciplineAdministeredBy { get; set; }
        [RequiredWhen(nameof(HasModifiedDiscipline), ComparisonType.EqualTo, true)]
        [DropDown, EntityMap, EntityMustExist(typeof(AccountabilityActionTakenType))]
        public virtual int? ModifiedAccountabilityActionTakenType { get; set; }
        [RequiredWhen("HasModifiedDiscipline", ComparisonType.EqualTo, true)]
        [Multiline]
        [StringLength(EmployeeAccountabilityAction.StringLengths.ACCOUNTABILITY_ACTION_TAKEN_DESCRIPTION)]
        public virtual string ModifiedAccountabilityActionTakenDescription { get; set; }
        [RequiredWhen("HasModifiedDiscipline", ComparisonType.EqualTo, true)]
        public virtual DateTime? DateModified { get; set; }
        public virtual DateTime? ModifiedStartDate { get; set; }
        public virtual DateTime? ModifiedEndDate { get; set; }
        public virtual int? ModifiedNumberOfWorkDays { get; set; }
        public virtual bool BackPayRequired { get; set; }

        #endregion

        #endregion

        #region Constructor

        public EmployeeAccountabilityActionViewModel(IContainer container) : base(container) { }

        #endregion
    }
}