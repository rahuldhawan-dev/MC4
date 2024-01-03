using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TrainingSessionControllerTest : MapCallMvcControllerTestBase<TrainingSessionController, TrainingSession>
    {
        #region Fields

        private User _user;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditTrainingSession)vm;
                return new { action = "Show", controller = "TrainingRecord", id = model.TrainingRecordId };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = TrainingSessionController.ROLE;
            Authorization.Assert(a => {
                a.RequiresRole("~/TrainingSession/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/TrainingSession/Update/", role, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create();
            var eq = GetEntityFactory<TrainingSession>().Create(new { TrainingRecord = trainingRecord });

            var expected = Lambdas.GetNow();

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTrainingSession, TrainingSession>(eq, new {
                StartDateTime = expected
            }));

            Assert.AreEqual(expected, Session.Get<TrainingSession>(eq.Id).StartDateTime);
        }

        #endregion
    }
}