using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class CutoffSawQuestionnaireViewModel : ViewModel<CutoffSawQuestionnaire>
    {
        #region Properties

        [DisplayName("WorkOrderID")]
        [AutoComplete("FieldOperations", "WorkOrder", "FindByPartialWorkOrderIDMatch")]
        [EntityMustExist(typeof(WorkOrder))]
        [EntityMap("WorkOrder")]
        public virtual int? WorkOrderID { get; set; }
        public virtual string WorkOrderSAP { get; set; }
        [Required]
        [DropDown]
        [DisplayName("Lead Person")]
        [EntityMustExist(typeof(Employee))]
        [EntityMap]
        public virtual int? LeadPerson { get; set; }
        [Required]
        [DropDown]
        [DisplayName("Saw Operator")]
        [EntityMustExist(typeof(Employee))]
        [EntityMap]
        public virtual int? SawOperator { get; set; }
        [Required]
        public virtual DateTime? OperatedOn { get; set; }
        public virtual string Comments { get; set; }
        [DisplayName("Pipe Material")]
        [EntityMustExist(typeof(PipeMaterial))]
        [EntityMap]
        [DropDown]
        public virtual int? PipeMaterial { get; set; }
        [DisplayName("Pipe Diameter")]
        [EntityMustExist(typeof(PipeDiameter))]
        [EntityMap]
        [DropDown]
        public virtual int? PipeDiameter { get; set; }

        public virtual IList<CutoffSawQuestionViewModel> CutoffSawQuestions { get; set; }

        #endregion
        
        #region Constructors

        public CutoffSawQuestionnaireViewModel(IContainer container) : base(container)
        {
            CutoffSawQuestions = new List<CutoffSawQuestionViewModel>();
        }

        #endregion
    }

    public class CreateCutoffSawQuestionnaire : CutoffSawQuestionnaireViewModel
    {
        #region Constants

        public const string QUESTIONS_CHANGED = "The list of questions has changed, please review and submit again";
        
        #endregion

        #region Properties

        [DoesNotAutoMap]
        [RequiresConfirmation(ErrorMessage = "You must agree to the terms and conditions.")]
        public virtual bool Agree { get; set; }

        #endregion
        
        #region Constructors

        public CreateCutoffSawQuestionnaire(IContainer container) : base(container)
        {
            CutoffSawQuestions = CutoffSawQuestions ?? new List<CutoffSawQuestionViewModel>();
        }

        #endregion

        #region Exposed Methods
        
        public override CutoffSawQuestionnaire MapToEntity(CutoffSawQuestionnaire entity)
        {
            entity = base.MapToEntity(entity);
            var questionRepository = _container.GetInstance<ICutoffSawQuestionRepository>();
            foreach (var q in CutoffSawQuestions)
            {
                entity.CutoffSawQuestions.Add(questionRepository.Find(q.Id));
            }
            entity.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var activeQuestions = _container.GetInstance<ICutoffSawQuestionRepository>().GetActiveQuestions().Select(x => x.Id).ToList();
            var currentQuestions = CutoffSawQuestions.Select(x => x.Id).ToList();

            if (activeQuestions.Except(currentQuestions).Any() || 
                currentQuestions.Except(activeQuestions).Any())
                yield return new ValidationResult(string.Format(QUESTIONS_CHANGED));
        }

        #endregion
    }

    public class SearchCutoffSawQuestionnaire : SearchSet<CutoffSawQuestionnaire>
    {
        #region Properties

        public virtual DateRange OperatedOn { get; set; }

        #endregion
    }
}