using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations._2016;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class ApcInspectionItemViewModel : ViewModel<ApcInspectionItem>
    {
        #region Properties

        [DropDown, Required, EntityMustExist(typeof(OperatingCenter)), EntityMap]
        public int? OperatingCenter { get; set; }
        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter"), Required, EntityMustExist(typeof(Facility)), EntityMap]
        public int? Facility { get; set; }
        [Required, StringLength(CreateAPCInspectionItemsTableForBug2891.StringLengths.AREA)]
        public string Area { get; set; }
        [Required, StringLength(CreateAPCInspectionItemsTableForBug2891.StringLengths.DESCRIPTION)]
        public string Description { get; set; }
        [Required, DropDown, EntityMustExist(typeof(ApcInspectionItemType)), EntityMap]
        public int? Type { get; set; }
        [Required]
        public DateTime? DateReported { get; set; }
        public DateTime? DateInspected { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FacilityInspectionRatingType))]
        public int? FacilityInspectionRatingType { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(FacilityInspectionAreaType))]
        [View(ApcInspectionItem.DisplayNames.FACILITY_AREA)]
        public int[] FacilityInspectionAreaTypes { get; set; }

        public int? Score { get; set; }

        public string Percentage { get; set; }

        #endregion

        #region Fields

        protected readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Questions

        [DoesNotAutoMap]
        public List<CreateApcFormAnswer> CreateApcFormAnswers { get; set; }

        #endregion

        #region Constructors

        public ApcInspectionItemViewModel(IContainer container) : base(container)
        {
            CreateApcFormAnswers = new List<CreateApcFormAnswer>();
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
        }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            SetDefaultQuestionsAnswers();
        }

        protected void SetDefaultQuestionsAnswers()
        {
            if (CreateApcFormAnswers.Count == 0)
            {
                var questions = _container.GetInstance<IRepository<FacilityInspectionFormQuestion>>().GetAll()
                                          .ToList();
                foreach (var question in questions)
                {
                    var answer = new CreateApcFormAnswer(_container) {
                        ApcFormId = Id,
                        FacilityInspectionFormQuestion = question.Id,
                        FacilityInspectionFormQuestionDisplay = question,
                        Category = question.Category.Id
                    };
                    CreateApcFormAnswers.Add(answer);
                }
            }
        }

        public override ApcInspectionItem MapToEntity(ApcInspectionItem entity)
        {
            base.MapToEntity(entity);
            MapAnswersToEntity(entity);
            return entity;
        }

        private void MapAnswersToEntity(ApcInspectionItem entity)
        {
            var questionRepository = _container.GetInstance<IRepository<FacilityInspectionFormQuestion>>();
            foreach (var answer in CreateApcFormAnswers)
            {
                if (entity.FacilityInspectionFormAnswers.FirstOrDefault(x =>
                    x.FacilityInspectionFormQuestion.Id == answer.FacilityInspectionFormQuestion) == null)
                {
                    entity.FacilityInspectionFormAnswers.Add(new FacilityInspectionFormAnswer {
                        ApcInspectionItem = entity,
                        FacilityInspectionFormQuestion = new FacilityInspectionFormQuestion{ Id = answer.FacilityInspectionFormQuestion },
                        IsPictureTaken = answer.IsPictureTaken,
                        Comments = answer.Comments,
                        IsSafe = answer.IsSafe
                    });
                }
            }
        }

        #endregion
    }

    public class CreateApcInspectionItem : ApcInspectionItemViewModel
    {
        #region Properties

        [Required, DropDown("", "Employee", "ActiveFieldServicesWorkManagementEmployeesByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Employee))]
        public int? AssignedTo { get; set; }

        #endregion

        #region Constructors

        public CreateApcInspectionItem(IContainer container) : base(container)
        {
            CreateApcFormAnswers = new List<CreateApcFormAnswer>();
        }

        #endregion
    }

    public class CreateApcFormAnswer : FacilityInspectionFormAnswerViewModel
    {
        #region Constructors

        public CreateApcFormAnswer(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public int? ApcFormId { get; set; }

        #endregion
    }

    public class EditApcInspectionItem : ApcInspectionItemViewModel
    {
        #region Fields

        protected readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public List<EditApcFormAnswer> EditApcFormAnswers { get; set; }

        [Required, DropDown("", "Employee", "ActiveFieldServicesWorkManagementEmployeesByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Employee))]
        public int? AssignedTo { get; set; }

        #endregion

        #region Constructors

        public EditApcInspectionItem(IContainer container) : base(container)
        {
            EditApcFormAnswers = new List<EditApcFormAnswer>();
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
        }

        #endregion

        #region Private Methods

        private void MapAnswersToEntity(ApcInspectionItem entity)
        {
            foreach (var answer in EditApcFormAnswers)
            {
                var answerViewModel = entity.FacilityInspectionFormAnswers.FirstOrDefault(x => x.Id == answer.Id);
                if (answerViewModel != null)
                {
                    answerViewModel.IsPictureTaken = answer?.IsPictureTaken;
                    answerViewModel.IsSafe = answer?.IsSafe;
                    answerViewModel.Comments = answer?.Comments;
                }
            }
        }

        #endregion

        #region Exposed Methods

        public override void Map(ApcInspectionItem entity)
        {
            base.Map(entity);
            EditApcFormAnswers = entity.FacilityInspectionFormAnswers
                                       .Select(x => _viewModelFactory.BuildWithOverrides<EditApcFormAnswer, FacilityInspectionFormAnswer>(x, new {
                                            Category = x.FacilityInspectionFormQuestion.Category.Id,
                                            FacilityInspectionFormQuestionDisplay = x.FacilityInspectionFormQuestion,
                                            Comments = x.Comments,
                                            Id = x.Id,
                                            IsSafe = x.IsSafe,
                                            IsPictureTaken = x.IsPictureTaken
                                        })).ToList();
        }

        public override ApcInspectionItem MapToEntity(ApcInspectionItem entity)
        {
            base.MapToEntity(entity);
            MapAnswersToEntity(entity);
            return entity;
        }

        #endregion
    }

    public class SearchApcInspectionItem : SearchSet<ApcInspectionItem>
    {
        #region Properties

        [EntityMap, EntityMustExist(typeof(State))]
        [DropDown, SearchAlias("OperatingCenter", "State.Id", Required = true)]
        public virtual int? State { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdOrAll", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }
        [DropDown("", "Facility", "ByOperatingCenterIds", DependsOn = "OperatingCenter"), EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ApcInspectionItemType))]
        public int? Type { get; set; }
        public DateRange DateReported { get; set; }
        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterIds", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(Employee))]
        public int? AssignedTo { get; set; }
        public DateRange DateInspected { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(FacilityInspectionAreaType))]
        [View(ApcInspectionItem.DisplayNames.FACILITY_AREA)]
        [SearchAlias("FacilityInspectionAreaTypes", "Id")]
        public int[] FacilityInspectionAreaTypes { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FacilityInspectionRatingType))]
        [View(ApcInspectionItem.DisplayNames.INSPECTION_RATING)]
        public int? FacilityInspectionRatingType { get; set; }

        #endregion
    }
}