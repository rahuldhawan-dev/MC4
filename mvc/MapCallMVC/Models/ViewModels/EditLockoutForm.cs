using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2016;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class EditLockoutForm : LockoutFormViewModel
    {
        #region Fields

        protected readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public List<EditLockoutFormAnswer> EditLockoutFormAnswers { get; set; }

        #region RETURNED TO SERVICE

        [DateTimePicker]
        public virtual DateTime? ReturnedToServiceDateTime { get; set; }

        [EntityMap, EntityMustExist(typeof(Employee))]
        [DropDown("", "Employee", "ActiveLockoutFormEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [RequiredWhen("ReturnedToServiceDateTime", ComparisonType.NotEqualTo, null, ErrorMessage = "The Return To Service Authorized Employee field is required.")]
        public virtual int? ReturnToServiceAuthorizedEmployee { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 5), Multiline]
        [RequiredWhen("ReturnedToServiceDateTime", ComparisonType.NotEqualTo, null, ErrorMessage = "The Returned To Service Notes field is required.")]
        public virtual string ReturnedToServiceNotes { get; set; }

        [RequiredWhen("ReturnedToServiceDateTime", ComparisonType.NotEqualTo, null, ErrorMessage = "The Same As Installer field is required.")]
        public virtual bool? SameAsInstaller { get; set; }

        [DropDown("", "Employee", "ActiveLockoutFormEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMap, RequiredWhen("SameAsInstaller", ComparisonType.EqualTo, false, ErrorMessage = "The Authorized Management Person field is required."), EntityMustExist(typeof(Employee))]
        public int? AuthorizedManagementPerson { get; set; }
        [EntityMap, DropDown, RequiredWhen("SameAsInstaller", ComparisonType.EqualTo, false, ErrorMessage = "The Lock Removed By field is required."), EntityMustExist(typeof(WayToRemoveLocks))]
        public int? LockRemovalMethod { get; set; }

        [EntityMap, DropDown("", "Employee", "ActiveLockoutFormEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [RequiredWhen("SameAsInstaller", ComparisonType.EqualTo, false, ErrorMessage = "The Supervisor Involved field is required."), EntityMustExist(typeof(Employee))]
        public int? SupervisorInvolved { get; set; }
        [RequiredWhen("SameAsInstaller", ComparisonType.EqualTo, false, ErrorMessage = "The Date of Contact field is required."), DateTimePicker]
        public DateTime? DateOfContact { get; set; }
        [RequiredWhen("SameAsInstaller", ComparisonType.EqualTo, false, ErrorMessage = "The Method of Contact field is required."), StringLength(LockoutForm.StringLengths.METHOD_OF_CONTACT)]
        public string MethodOfContact { get; set; }
        [RequiredWhen("SameAsInstaller", ComparisonType.EqualTo, false, ErrorMessage = "The Outcome of Contact field is required."), Multiline]
        public string OutcomeOfContact { get; set; }

        #endregion

        #endregion

        #region Constructors

        public EditLockoutForm(IContainer container) : base(container)
        {
            EditLockoutFormAnswers = new List<EditLockoutFormAnswer>();
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
        }

        #endregion

        #region Private Methods
        
        private void MapAnswersToEntity(LockoutForm entity)
        {
            var questionRepository = _container.GetInstance<IRepository<LockoutFormQuestion>>();

            foreach (var answer in EditLockoutFormAnswers)
            {
                var answerViewModel = entity.LockoutFormAnswers.FirstOrDefault(x => x.Id == answer.Id);
                answerViewModel.Answer = answer.Answer;
                answerViewModel.LockoutFormQuestion = questionRepository.Find(answer.LockoutFormQuestion);
                answerViewModel.Comments = answer.Comments;
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        
        public override void Map(LockoutForm entity)
        {
            base.Map(entity);
            EditLockoutFormAnswers = entity.LockoutFormAnswers
                .Select(x => _viewModelFactory.BuildWithOverrides<EditLockoutFormAnswer, LockoutFormAnswer>(x, new {
                    Category = x.LockoutFormQuestion.Category.Id,
                    LockoutFormQuestionDisplay = x.LockoutFormQuestion
                })).ToList();
        }

        public override LockoutForm MapToEntity(LockoutForm entity)
        {
            base.MapToEntity(entity);
            MapAnswersToEntity(entity);
            return entity;
        }

        #region Validation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateManagementAnswers()).Concat(ValidateOutOfServiceAnswers());
        }

        private IEnumerable<ValidationResult> ValidateManagementAnswers()
        {
            //If same as installer, we don't need them to answer management questions.
            if (SameAsInstaller == null || SameAsInstaller.Value)
                yield break;

            foreach (var a in LockoutFormAnswers)
            {
                if (a.Category == LockoutFormQuestionCategory.Indices.MANAGEMENT && !a.Answer.HasValue)
                {
                    yield return new ValidationResult(ValidationErrors.MANAGEMENT, new[] { "LockoutFormAnswers" });
                }
            }
        }

        private IEnumerable<ValidationResult> ValidateOutOfServiceAnswers()
        {
            if (!ReturnedToServiceDateTime.HasValue)
                yield break;

            foreach (var a in LockoutFormAnswers)
            {
                if (a.Category == LockoutFormQuestionCategory.Indices.RETURN_TO_SERVICE && !a.Answer.HasValue)
                {
                    yield return new ValidationResult(ValidationErrors.OUT_OF_SERVICE, new[] { "LockoutFormAnswers" });
                }
            }
        }

        #endregion

        #endregion
    }
}