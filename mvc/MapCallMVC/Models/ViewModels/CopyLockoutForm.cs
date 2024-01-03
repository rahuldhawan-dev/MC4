using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CopyLockoutForm : CreateLockoutForm
    {
        #region Exposed Methods

        public override void SetDefaults()
        {
            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            OutOfServiceDateTime = now;
            LockoutDateTime = now;
            OutOfServiceAuthorizedEmployee = currentUser.Employee?.Id;
        }

        public override void Map(LockoutForm entity)
        {
            base.Map(entity);
            EmployeeAcknowledgedTraining = false;
            LocationOfLockoutNotes = null;
            IsolationPoint = null;
            IsolationPointDescription = null;
            LockoutDevice = null;
            MapQuestionsAnswers(entity);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add all the questions/answers from the copied form
        /// If an answer's question's category is a NEW_CATEGORY, copy the answer
        /// otherwise null out the answer.
        /// </summary
        private void MapQuestionsAnswers(LockoutForm entity)
        {
            // Add all the existing questions with answers
            // if the category is a new category, copy the answer, otherwise null the answer
            foreach (var answer in entity.LockoutFormAnswers)
            {
                CreateLockoutFormAnswers.Add(
                    _viewModelFactory.BuildWithOverrides<CreateLockoutFormAnswer, LockoutFormAnswer>(answer, new {
                        Answer = (LockoutFormQuestionCategory.NEW_CATEGORIES.Contains(answer.LockoutFormQuestion.Category.Id)) ? answer.Answer : null,
                        LockoutFormQuestionDisplay = answer.LockoutFormQuestion,
                        Category = answer.LockoutFormQuestion.Category.Id
                    }));
            }
        }
        
        #endregion

        #region Constructors

        public CopyLockoutForm(IContainer container) : base(container) { }

        #endregion
    }
}