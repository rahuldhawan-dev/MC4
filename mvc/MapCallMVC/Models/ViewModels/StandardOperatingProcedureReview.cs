using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateStandardOperatingProcedureReview : ViewModel<StandardOperatingProcedureReview>
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(StandardOperatingProcedure))]
        public int? StandardOperatingProcedure { get; set; }

        [AutoMap(MapDirections.None)]
        public List<CSOPRReviewAnswer> Questions { get; set; }

        #endregion

        #region Constructors

        public CreateStandardOperatingProcedureReview(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();

            var sop = _container.GetInstance<IRepository<StandardOperatingProcedure>>().Find(StandardOperatingProcedure.Value);

            Questions = new List<CSOPRReviewAnswer>();

            foreach (var q in sop.ActiveQuestions)
            {
                var qvm = new CSOPRReviewAnswer(_container);
                qvm.Question = q.Id;
                Questions.Add(qvm);
            }
        }

        public override StandardOperatingProcedureReview MapToEntity(StandardOperatingProcedureReview entity)
        {
            base.MapToEntity(entity);
            entity.AnsweredBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            entity.AnsweredAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            foreach (var q in Questions)
            {
                var actualQuestion = _container.GetInstance<IRepository<StandardOperatingProcedureQuestion>>().Find(q.Question.Value);

                // If a question is marked as not active while a user is currently filling it out, we don't
                // want to save the answer to that question since it will not be able to be marked as correct/incorrect.
                if (actualQuestion.IsActive)
                {
                    var qa = new StandardOperatingProcedureReviewAnswer();
                    qa.Answer = q.Answer;
                    qa.Question = actualQuestion;
                    qa.Review = entity;
                    entity.Answers.Add(qa);
                }
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // TODO: Validate that all and only active questions are added for the given SOP.
            return base.Validate(validationContext);
        }

        #endregion

        #region Helper classes

        public class CSOPRReviewAnswer
        {
            #region Fields

            private readonly IContainer _container;

            #endregion

            #region Properties

            public StandardOperatingProcedureQuestion ActualQuestion
            {
                get
                {
                    var repo = _container.GetInstance<IRepository<StandardOperatingProcedureQuestion>>();
                    if (Question.HasValue)
                    {
                        var q = repo.Find(Question.Value);

                        if (q != null)
                        {
                            return q;
                        }
                    }

                    return null;
                }
            }

            [Required, EntityMap, EntityMustExist(typeof(StandardOperatingProcedureQuestion))]
            public int? Question { get; set; }

            [Required, Multiline]
            public string Answer { get; set; }

            #endregion

            #region Constructor

            public CSOPRReviewAnswer(IContainer container)
            {
                _container = container;
            }

            #endregion
        }

        #endregion
    }

    public class EditStandardOperatingProcedureReview : ViewModel<StandardOperatingProcedureReview>
    {
        #region Properties

        [DoesNotAutoMap("Mapped manually.")]
        public List<CSOPRAnswerApproval> Questions { get; set; }
        
        [DoesNotAutoMap("Display only.")]
        public StandardOperatingProcedureReview OriginalEntity
        {
            get { return _container.GetInstance<IStandardOperatingProcedureReviewRepository>().Find(Id); }
        }

        #endregion

        #region Constructors

        public EditStandardOperatingProcedureReview(IContainer container) : base(container) {}

        #endregion

        #region Public Methods

        public override void Map(StandardOperatingProcedureReview entity)
        {
            base.Map(entity);

            Questions = new List<CSOPRAnswerApproval>();

            foreach (var q in entity.Answers)
            {
                Questions.Add(new CSOPRAnswerApproval(_container)
                {
                    Id = q.Id,
                    UserAnswer = q.Answer
                });
            }
        }

        public override StandardOperatingProcedureReview MapToEntity(StandardOperatingProcedureReview entity)
        {
            base.MapToEntity(entity);
            entity.ReviewedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            entity.ReviewedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            var modelAnswersById = Questions.ToDictionary(x => x.Id.Value, x => x);

            foreach (var answer in entity.Answers)
            {
                answer.IsCorrect = modelAnswersById[answer.Id].IsCorrect;
            }

            return entity;
        }

        #endregion

        #region Helper Classes

        public class CSOPRAnswerApproval
        {
            #region Fields

            private readonly IContainer _container;

            #endregion

            #region Properties

            [Required]
            public int? Id { get; set; }

            [Multiline]
            public string UserAnswer { get; set; }

            [Required]
            public bool? IsCorrect { get; set; }

            public StandardOperatingProcedureQuestion ActualQuestion
            {
                get
                {
                    var repo = _container.GetInstance<IRepository<StandardOperatingProcedureReviewAnswer>>();
                    var ra = repo.Find(Id.Value);
                    return ra.Question;
                }
            }
            
            [Multiline]
            public string CorrectAnswer
            {
                get { return ActualQuestion.Answer; }
            }

            #endregion

            #region Constructor

            public CSOPRAnswerApproval(IContainer container)
            {
                _container = container;
            }

            #endregion
        }

        #endregion
    }

    // Needed for extra view data but isn't used for posting back to the index.
    // Also this is only a SearchSet to get around the class restriction of ActionHelper.DoSearch
    public class SOPWrapModel : SearchSet<StandardOperatingProcedureReview>
    {
        public SearchStandardOperatingProcedureReview Search { get; set; }
        public IEnumerable<StandardOperatingProcedure> ReviewableProcedures { get; set; }
    }

    public class SearchStandardOperatingProcedureReview : SearchSet<StandardOperatingProcedureReview>
    {
        [Search(CanMap=false)]
        public bool? RequiresReview { get; set; }

         // Temporarily. This needs to exist for the ModifyValues thing to work.
        public int? AnsweredBy { get; set; }

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            if (RequiresReview.HasValue)
            {
                var requiresReview = RequiresReview.Value
                    ? SearchMapperSpecialValues.IsNull
                    : SearchMapperSpecialValues.IsNotNull;
                mapper.MappedProperties["AnsweredBy"].Value = requiresReview;
              
            }
        }
    }
}