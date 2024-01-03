using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HumanResources.Models.ViewModels
{
    public abstract class CovidIssueViewModel : ViewModel<CovidIssue>
    {
        #region Properties

        [StringLength(CovidIssue.StringLengths.SUPERVISORS_CELL)]
        [RegularExpression(RegularExpressions.PHONE, ErrorMessage = RegularExpressions.PHONE_ERROR_MESSAGE)]
        public string SupervisorsCell { get; set; }
        [StringLength(CovidIssue.StringLengths.LOCAL_ERBP)]
        public string LocalEmployeeRelationsBusinessPartner { get; set; }
        [StringLength(CovidIssue.StringLengths.LOCAL_ERBP_CELL)]
        [RegularExpression(RegularExpressions.PHONE, ErrorMessage = RegularExpressions.PHONE_ERROR_MESSAGE)]
        public string LocalEmployeeRelationsBusinessPartnerCell { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(CovidRequestType))]
        public int? RequestType { get; set; }
        [Required]
        public DateTime? SubmissionDate { get; set; }
        // AllowHtml Needed because users keep copy/pasting full emails into the field
        // and those include <user@email.com> text.
        [Required, Multiline, AllowHtml] 
        public string QuestionFromEmail { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(CovidSubmissionStatus))]
        public int? SubmissionStatus { get; set; }
        [RequiredWhen("SubmissionStatus", ComparisonType.EqualTo, CovidSubmissionStatus.Indices.COMPLETE)]
        public string OutcomeDescription { get; set; }
        [RequiredWhen("SubmissionStatus", ComparisonType.EqualTo, CovidSubmissionStatus.Indices.COMPLETE)]
        [DropDown, EntityMap, EntityMustExist(typeof(CovidOutcomeCategory))]
        public int? OutcomeCategory { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidQuarantineStatus))]
        public int? QuarantineStatus { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidAnswerType))]
        public int? WorkExposure { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidAnswerType))]
        public int? AvoidableCloseContact { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CovidAnswerType))]
        public int? FaceCoveringWorn { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ReleaseReason)), RequiredWhen("ReleaseDate", ComparisonType.NotEqualTo, null)]
        public int? ReleaseReason { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EstimatedReleaseDate { get; set; }
        public DateTime? ReleaseDate { get; set; }

        // AllowHtml here for the same reason as QuestionFromEmail.
        [Multiline, AllowHtml]
        public string QuarantineReason { get; set; }

        [StringLength(CovidIssue.StringLengths.PERSONAL_EMAIL_ADDRESS)]
        [MMSINC.Validation.EmailAddress]
        public string PersonalEmailAddress { get; set; }

        public bool? HealthDepartmentNotification { get; set; }

        #endregion

        #region Constructors

        public CovidIssueViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateCovidIssue : CovidIssueViewModel
    {
        #region Properties

        [Required, DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }
        [DropDown("", "Employee", "CovidIssueActiveEmployeesByState", DependsOn = "State", PromptText = "Please select a state.")]
        [Required, EntityMap, EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }

        #endregion

        #region Constructors

        public CreateCovidIssue(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override CovidIssue MapToEntity(CovidIssue entity)
        {
            var ret = base.MapToEntity(entity);
            ret.HumanResourcesManager = ret.Employee.HumanResourcesManager;
            ret.PersonnelArea = ret.Employee.PersonnelArea;

            return ret;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<ValidationResult> validateEmployeeHasPersonnelArea()
            {
                if (!Employee.HasValue)
                {
                    yield break; // exit early. handled by required validation.
                }
                var emp = _container.GetInstance<IRepository<Employee>>().Find(Employee.Value);
                if (emp.PersonnelArea == null)
                {
                    yield return new ValidationResult("The employee record must have an associated Personnel Area set.", new[] { nameof(Employee) });
                }
            }

            return base.Validate(validationContext).Concat(validateEmployeeHasPersonnelArea());
        }

        #endregion
    }

    public class EditCovidIssue : CovidIssueViewModel
    {
        #region Fields

        private Employee _employee;

        #endregion

        #region Properties

        [DoesNotAutoMap("Display only")]
        public Employee Employee
        {
            get
            {
                if (_employee == null)
                {
                    _employee = _container.GetInstance<IRepository<CovidIssue>>().Find(Id)?.Employee;
                }
                return _employee;
            }
        }

        #endregion

        #region Constructors

        public EditCovidIssue(IContainer container) : base(container) { }

        #endregion
    }
}
