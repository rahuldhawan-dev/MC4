using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Controllers
{
    [TestClass]
    public class RecurringProjectEndorsementControllerTest : MapCallMvcControllerTestBase<RecurringProjectEndorsementController, RecurringProjectEndorsement>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditRecurringProjectEndorsement)vm;
                return new { action = "Show", controller = "RecurringProject", area = "ProjectManagement", id = model.RecurringProject };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesProjects;
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/RecurringProjectEndorsement/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/RecurringProjectEndorsement/Update/", role, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var rp = GetEntityFactory<RecurringProject>().Create();
            var rpe = GetEntityFactory<RecurringProjectEndorsement>().Create(new { RecurringProject = rp });

            var result =
                _target.Update(
                    _viewModelFactory.BuildWithOverrides<EditRecurringProjectEndorsement, RecurringProjectEndorsement>(
                        rpe, new {Comment = "Foo"}));

            Assert.AreEqual("Foo", Session.Get<RecurringProjectEndorsement>(rpe.Id).Comment);
        }

        #endregion
    }
}