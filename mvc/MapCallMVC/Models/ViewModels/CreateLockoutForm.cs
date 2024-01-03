using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using System.Collections.Generic;
using System.Linq;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateLockoutForm : LockoutFormViewModel
    {
        #region Fields

        protected readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public List<CreateLockoutFormAnswer> CreateLockoutFormAnswers { get; set; }

        #endregion

        #region Constructors

        public CreateLockoutForm(IContainer container) : base(container)
        {
            CreateLockoutFormAnswers = new List<CreateLockoutFormAnswer>();
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
        }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            var operatingCenter = currentUser.DefaultOperatingCenter.Id;
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            OutOfServiceDateTime = now;
            LockoutDateTime = now;
            if (!OperatingCenter.HasValue)
            {
                OperatingCenter = operatingCenter;
            }
            OutOfServiceAuthorizedEmployee = currentUser.Employee?.Id;

            SetDefaultQuestionsAnswers();
        }

        protected void SetDefaultQuestionsAnswers()
        {
            if (CreateLockoutFormAnswers.Count == 0)
            {
                var questions = _container.GetInstance<IRepository<LockoutFormQuestion>>().GetActiveQuestionsForCreate()
                                          .ToList();
                foreach (var q in questions)
                {
                    var answer = new CreateLockoutFormAnswer(_container) {
                        LockoutForm = Id,
                        LockoutFormQuestion = q.Id,
                        LockoutFormQuestionDisplay = q,
                        Category = q.Category.Id
                    };
                    CreateLockoutFormAnswers.Add(answer);
                }
            }
            else
            {
                PopulateDisplayQuestionsForAnswers();
            }
        }

        private void PopulateDisplayQuestionsForAnswers()
        {
            var questionRepo = _container.GetInstance<IRepository<LockoutFormQuestion>>();
            foreach (var answer in CreateLockoutFormAnswers)
            {
                var question = questionRepo.Find(answer.LockoutFormQuestion);
                answer.LockoutFormQuestionDisplay = question;
                answer.Category = question.Category.Id;
            }
        }

        public override void Map(LockoutForm entity)
        {
            base.Map(entity);
        }

        public override LockoutForm MapToEntity(LockoutForm entity)
        {
            base.MapToEntity(entity);

            if (entity.ProductionWorkOrder != null && entity.ProductionWorkOrder.ProductionWorkOrderProductionPrerequisites.Any(x => x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT))
            {
                var dateTimeProvider = _container.GetInstance<IDateTimeProvider>();

                var existingLockOutPreReq = entity.ProductionWorkOrder.ProductionWorkOrderProductionPrerequisites.Where(x =>
                     x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT).FirstOrDefault();

                existingLockOutPreReq.SatisfiedOn = dateTimeProvider.GetCurrentDate();
            }

            AddEditActiveQuestionAnswers(entity);
            MapAnswersToEntity(entity);
            return entity;
        }

        private void AddEditActiveQuestionAnswers(LockoutForm entity)
        {
            var activeQuestions = _container.GetInstance<IRepository<LockoutFormQuestion>>().GetActiveQuestions();
            foreach (var question in activeQuestions)
            {
                if (CreateLockoutFormAnswers.All(x => x.LockoutFormQuestion != question.Id))
                {
                    CreateLockoutFormAnswers.Add(
                        _viewModelFactory.BuildWithOverrides<CreateLockoutFormAnswer>(new {
                            LockoutForm = Id,
                            LockoutFormQuestion = question.Id,
                            Category = question.Category.Id
                        }));
                }
            }
        }

        private void MapAnswersToEntity(LockoutForm entity)
        {
            var questionRepository = _container.GetInstance<IRepository<LockoutFormQuestion>>();
            foreach (var answer in CreateLockoutFormAnswers)
            {
                entity.LockoutFormAnswers.Add(new LockoutFormAnswer {
                    LockoutForm = entity,
                    LockoutFormQuestion = questionRepository.Find(answer.LockoutFormQuestion),
                    Answer = answer.Answer,
                    Comments = answer.Comments
                });
            }
        }

        #endregion

        public void SetValuesFromProductionWorkOrder(ProductionWorkOrder order)
        {
            ProductionWorkOrder = order.Id;
            OperatingCenter = order.OperatingCenter?.Id;
            Facility = order.Facility?.Id ?? order.Equipment?.Facility?.Id;
            EquipmentType = order.EquipmentType?.Id;
            Equipment = order.Equipment?.Id;
        }
    }
}