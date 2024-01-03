using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EnvironmentalNonComplianceEventActionItemControllerTest : MapCallMvcControllerTestBase<EnvironmentalNonComplianceEventActionItemController, EnvironmentalNonComplianceEventActionItem>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditEnvironmentalNonComplianceEventActionItem)vm;
                model.ResponsibleOwner = GetEntityFactory<User>().Create().Id;
            };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditEnvironmentalNonComplianceEventActionItem)vm;
                return new { action = "Show", controller = "EnvironmentalNonComplianceEvent", area = "Environmental", id = model.EnvironmentalNonComplianceEventId };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventActionItem/Search/", RoleModules.EnvironmentalGeneral, RoleActions.Read);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventActionItem/Index/", RoleModules.EnvironmentalGeneral, RoleActions.Read);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventActionItem/Edit/", RoleModules.EnvironmentalGeneral, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EnvironmentalNonComplianceEventActionItem/Update/", RoleModules.EnvironmentalGeneral, RoleActions.Edit);
            });
        }
        
        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<EnvironmentalNonComplianceEventActionItem>().Create(new { ActionItem = "ActionItem 0" });
            var entity1 = GetEntityFactory<EnvironmentalNonComplianceEventActionItem>().Create(new { ActionItem = "ActionItem 1" });
            var search = new SearchEnvironmentalNonComplianceEventActionItem();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ActionItem, "ActionItem");
                helper.AreEqual(entity1.ActionItem, "ActionItem", 1);
                helper.AreEqual(entity0.EnvironmentalNonComplianceEvent.OperatingCenter, "OperatingCenter");
                helper.AreEqual(entity1.EnvironmentalNonComplianceEvent.OperatingCenter, "OperatingCenter", 1);
                helper.AreEqual(entity0.EnvironmentalNonComplianceEvent.State, "State");
                helper.AreEqual(entity1.EnvironmentalNonComplianceEvent.State, "State", 1);
                helper.AreEqual(entity0.EnvironmentalNonComplianceEvent.Id, "EnvironmentalNonComplianceEventId");
                helper.AreEqual(entity1.EnvironmentalNonComplianceEvent.Id, "EnvironmentalNonComplianceEventId", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EnvironmentalNonComplianceEventActionItem>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEnvironmentalNonComplianceEventActionItem, EnvironmentalNonComplianceEventActionItem>(eq, new {
                ActionItem = expected
            })) as RedirectToRouteResult;

            Assert.AreEqual(expected, Session.Get<EnvironmentalNonComplianceEventActionItem>(eq.Id).ActionItem);
        }

        #endregion
    }
}