using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Controllers
{
    [TestClass]
    public class RecurringProjectPipeDataLookupValueControllerTest : MapCallMvcControllerTestBase<RecurringProjectPipeDataLookupValueController, RecurringProjectPipeDataLookupValue>
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
                var model = (EditRecurringProjectPipeDataLookupValue)vm;
                return new { action = "Show", controller = "RecurringProject", area = "ProjectManagement", id = model.RecurringProject };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesProjects;
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/RecurringProjectPipeDataLookupValue/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/RecurringProjectPipeDataLookupValue/Update/", role, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var rp = GetEntityFactory<RecurringProject>().Create();
            var pdlv = GetEntityFactory<PipeDataLookupValue>().CreateList(2);
            var rppdlv = GetEntityFactory<RecurringProjectPipeDataLookupValue>().Create(new { RecurringProject = rp, PipeDataLookupValue = pdlv[0] });

            var result =
                _target.Update(
                    _viewModelFactory
                       .BuildWithOverrides<EditRecurringProjectPipeDataLookupValue, RecurringProjectPipeDataLookupValue
                        >(rppdlv, new {PipeDataLookupValue = pdlv[1].Id}));

            Assert.AreEqual(pdlv[1], Session.Get<RecurringProjectPipeDataLookupValue>(rppdlv.Id).PipeDataLookupValue);
        }

        #endregion
    }
}